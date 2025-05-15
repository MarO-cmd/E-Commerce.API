using Store.Maro.Core;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Entities.Identity;
using Store.Maro.Core.Entities.Orders;
using Store.Maro.Core.Repository.Contract;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Core.Specifications;
using Store.Maro.Core.Specifications.Orders;
using Store.Maro.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Services.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IUnitOfWork unitOfWork,
            IBasketService basketService,
            IPaymentService paymentService
            )
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAdress)
        {

            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var orderItems = new List<OrderItem>(); 
            if(basket.Items.Count > 0 )
            {
                    // we did that cuz may be the data of the basket is not correct spec price so we get it from the db 
                foreach (var productItem in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(productItem.Id);
                    var productOrderItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrderItem, product.Price, productItem.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(deliveryMethodId);

            // if there exist paymentIntent delete it cuz not to be 2 orders with the same paymentIntent
            if(!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderPaymentIntentSpecifications(basket.PaymentIntentId);
                var existOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                _unitOfWork.Repository<Order, int>().Delete(existOrder);
            }


            var customerBasketDto =  await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

            var order = new Order()
            { 
                BuyerEmail = buyerEmail,
                ShippingAddress = shippingAdress,
                Items = orderItems,
                DeliveryMethod= deliveryMethod,
                SubTotal = orderItems.Sum( I => I.Price * I.Quantity),
                PaymentIntentId =  customerBasketDto.PaymentIntentId // todo
                
            };

             
            await _unitOfWork.Repository<Order, int>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0  ? order : null;
        }

       

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);

            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (order is null) return null;

            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);

             var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if (orders is null) return null;

            return orders;
        }
    }
}
