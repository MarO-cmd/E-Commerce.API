using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface ICashService
    {
        public Task SetCachKeyAsync(string key, object response, TimeSpan expireTime);
        public Task<string> GetCachKeyAsync(string key);
    }
}
