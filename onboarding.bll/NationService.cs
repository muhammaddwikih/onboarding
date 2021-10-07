using Microsoft.EntityFrameworkCore;
using onboarding.bll.Cache;
using onboarding.dal.Model;
using onboarding.dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onboarding.bll
{
    public class NationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redis;

        public NationService(IUnitOfWork unitOfWork, IRedisService redis)
        {
            _unitOfWork = unitOfWork;
            _redis = redis;
        }

        public List<National> GetAll()
        {
            return _unitOfWork.NationalRepository.GetAll().Include(x => x.Movie).ToList();
        }

        public async Task<National> GetByName(string name)
        {
            National national = await _redis.GetAsync<National>($"national_nationalName:{name}");

            if (national == null)
            {
                national = await _unitOfWork.NationalRepository.GetAll().Include(t => t.Movie).FirstOrDefaultAsync(x => x.NationName == name);

                await _redis.SaveAsync($"national_nationalName:{name}", national);
            }

            return national;
        }

        public void CreateNation(National mapperResult)
        {
            bool IsNationExist = _unitOfWork.NationalRepository.GetAll().Where(x => x.NationName == mapperResult.NationName).Any();
            if (!IsNationExist)
            {
                _unitOfWork.NationalRepository.Add(mapperResult);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception($"National with {mapperResult.NationName} already exist");
            }
        }

        public async Task EditNational(Guid id, National mapperResult)
        {
            bool IsNationExist = _unitOfWork.NationalRepository.GetAll().Where(x => x.Id == id).Any();
            if (IsNationExist)
            {
                mapperResult.Id = id;
                _unitOfWork.NationalRepository.Edit(mapperResult);
                await _unitOfWork.SaveAsync();
                await _redis.DeleteAsync($"national_nationalName:{mapperResult.NationName}");
            }
            else
            {
                throw new Exception($"National with {id} not found");
            }
        }

        public async Task Delete(Guid id)
        {
            var name = _unitOfWork.NationalRepository.GetAll().Where(x => x.Id == id).Select(t => t.NationName);
            _unitOfWork.NationalRepository.Delete(x => x.Id == id);
            await _unitOfWork.SaveAsync();
            await _redis.DeleteAsync($"national_nationalName:{name}");
        }
    }
}
