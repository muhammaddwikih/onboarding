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

        public MovieService(IUnitOfWork unitOfWork, IConfiguration config, IkafkaSender kafkaSender)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _kafkaSender = kafkaSender;
        }

        public List<MovieModel> GetAll()
        {
            return _unitOfWork.MovieRepository.GetAll().Include(x => x.Nation).ToList();
        }

        public void DeleteMovie(string title)
        {
            _unitOfWork.MovieRepository.Delete(x => x.Title == title);
            _unitOfWork.Save();
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

        public void EditMovie(Guid id, MovieModel movie)
        {
            bool IsMovieExist = _unitOfWork.MovieRepository.GetAll().Where(x => x.Id == id).Any();
            if (IsMovieExist) {
                movie.Id = id;
                _unitOfWork.MovieRepository.Edit(movie);
                _unitOfWork.Save();
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
