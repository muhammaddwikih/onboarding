using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using onboarding.bll.Kafka;
using onboarding.dal.Model;
using onboarding.dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onboarding.bll
{
    public class MovieService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IkafkaSender _kafkaSender;

        public MovieService(IUnitOfWork unitOfWork, IkafkaSender kafkaSender, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _kafkaSender = kafkaSender;
        }

        public List<MovieModel> GetAll()
        {
/*            List<int> a = new List<int>();
            a.Add(1);
            a.Add(1);
            a.Add(2);
            a.Add(3);


            Dictionary<int, int> ss = new Dictionary<int, int>();
            ss.Add(5, 3);

            int[] sss = new int[] { 1, 2, 3, 4, 5 };
            
            var b = a.Where(x => x.Equals(1)).Count();
            Console.WriteLine(sss.Min());

            var z = sss.GetValue(1);
            Console.WriteLine(z);
            int asd = ss.FirstOrDefault(x => x.Value == 3).Key;

            sss.Where(x => 7 <= x && x <= 10).Count();*/
            
            return _unitOfWork.MovieRepository.GetAll().Include(x => x.Nation).ToList();
        }

        public async Task DeleteMovie(string title)
        {
            _unitOfWork.MovieRepository.Delete(x => x.Title == title);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateMovie(MovieModel value)
        {
            bool IsMovieExist = _unitOfWork.MovieRepository.GetAll().Where(x => x.Title == value.Title).Any();
            bool isNationalExist = _unitOfWork.NationalRepository.GetAll().Where(x => x.Id == value.NationalId).Any();
            if (!IsMovieExist && isNationalExist)
            {
                _unitOfWork.MovieRepository.Add(value);
                await _unitOfWork.SaveAsync();
                await SendMovieToKafka(value);
            }
            else
            {
                throw new Exception($"Movie with {value.Id} already exist");
            }
        }

        public MovieModel GetMovieById(int imdbId)
        {
            return _unitOfWork.MovieRepository.GetAll().Where(x => x.ImdbId == imdbId).FirstOrDefault();
        }

        public async Task EditMovie(Guid id, MovieModel movie)
        {
            bool IsMovieExist = _unitOfWork.MovieRepository.GetAll().Where(x => x.Id == id).Any();
            if (IsMovieExist) {
                movie.Id = id;
                _unitOfWork.MovieRepository.Edit(movie);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception($"Movie with {id} not found");
            }
        }

        private async Task SendMovieToKafka(MovieModel movie)
        {
            string topic = _config.GetValue<string>("Topic:Movie");
            await _kafkaSender.SendAsync(topic, movie);
        }
    }
}
