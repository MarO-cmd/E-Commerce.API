using Microsoft.Extensions.Configuration;
using Store.Maro.Core;
using Store.Maro.Core.Dtos.Baskets;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Entities.Orders;
using Store.Maro.Core.Services.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Maro.Core.Entities.Product;

namespace Store.Maro.Services.Services.Payments
{
    public class PaymentServices : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentServices(
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IConfiguration configuration
            )
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {

            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            // check basket
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            // check price
            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);
                    if (product.Price != item.Price)
                        item.Price = product.Price;
                }
            }

            // calculate Total
            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }


            var service = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // create 
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)((subTotal + shippingPrice) * 100),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "usd"
                };

                // create payment using stripe
                var paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // update 

                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)((subTotal + shippingPrice) * 100),
                };

                // update payment using stripe
                var paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }

            //update basket
            basket = await _basketService.SetBasketAsync(basket);

            if (basket is null) return null;

            return basket;

        }
    }
}
