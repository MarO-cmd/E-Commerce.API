using StackExchange.Redis;
using Store.Maro.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Maro.Services.Services.CacheServices
{
    public class CachService : ICashService
    {
        private readonly IDatabase _dataBase;

        public CachService(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }

        public async Task<string> GetCachKeyAsync(string key)
        {
            var response = await _dataBase.StringGetAsync(key);

            return response.IsNullOrEmpty ? null : response.ToString();
        }

        public async Task SetCachKeyAsync(string key, object response, TimeSpan expireTime)
        {
            // this configure to u some options for json
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await _dataBase.StringSetAsync(key, JsonSerializer.Serialize(response,options), expireTime);

        }

    }
}
