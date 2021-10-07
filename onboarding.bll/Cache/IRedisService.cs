using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace onboarding.bll.Cache
{
    public interface IRedisService
    {
        Task SaveAsync(string key, object value);
        Task<T> GetAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
    }
}
