using StackExchange.Redis;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Maro.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _DataBase;

        public BasketRepository(IConnectionMultiplexer redis )
        {
            _DataBase = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _DataBase.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var basket = await _DataBase.StringGetAsync(id);


            // resdis value is just like json so u need to convert it to object so u use deserialize
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {

            var res = await _DataBase.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(14));

            return (res ? await GetBasketAsync(basket.Id) : null);
        }

    }
}
