using Store.Maro.Core.Dtos.Baskets;
using Store.Maro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface IPaymentService
    {
        public Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
