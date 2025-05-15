using AutoMapper;
using Store.Maro.Core.Dtos.Baskets;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Repository.Contract;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Services.Services.BasketServices
{
    public class BasketService :IBasketService
    {
        private readonly IBasketRepository _basketReposiory;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketReposiory ,IMapper mapper)
        {
            _basketReposiory = basketReposiory;
            _mapper = mapper;
        }

        public async Task<CustomerBasketDto?> GetBasketAsync(string id)
        {

            var basket = await _basketReposiory.GetBasketAsync(id);

            // if basket is null so there is no such an id exist so make new basket with that id 
            if (basket is null) return await SetBasketAsync(new CustomerBasketDto() { Id = id });

          
            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            var basket = await _basketReposiory.DeleteBasketAsync(id);

            return basket;
        }
        public async Task<CustomerBasketDto> SetBasketAsync(CustomerBasketDto model)
        {

            var basket = _mapper.Map<CustomerBasket>(model);

            var res = await _basketReposiory.UpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasketDto>(res);
        }
        



    }
}
