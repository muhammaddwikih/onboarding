using Microsoft.EntityFrameworkCore;
using onboarding.dal.Model;
using onboarding.dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace onboarding.bll
{
    public class MovieService
    {

        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public void CreateMovie(MovieModel value)
        {
            bool IsMovieExist = _unitOfWork.MovieRepository.GetAll().Where(x => x.Title == value.Title).Any();
            bool isNationalExist = _unitOfWork.NationalRepository.GetAll().Where(x => x.Id == value.NationalId).Any();
            if (!IsMovieExist && isNationalExist)
            {
                _unitOfWork.MovieRepository.Add(value);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception($"Movie with {value.Id} already exist");
            }
        }
    }
}
