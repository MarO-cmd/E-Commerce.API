using Store.Maro.Core.Dtos.Baskets;
using Store.Maro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface IBasketService
    {
        public Task<CustomerBasketDto?> GetBasketAsync(string id);
        public Task<bool> DeleteBasketAsync(string id);
        public Task<CustomerBasketDto> SetBasketAsync(CustomerBasketDto model);

    }
}
