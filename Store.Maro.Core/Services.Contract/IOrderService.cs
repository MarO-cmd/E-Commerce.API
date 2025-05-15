using Store.Maro.Core.Entities.Identity;
using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(string buyerEmail ,string basketId ,int deliveryMethodId, Address shippingAdress );
        public Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail);
        public Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail , int orderId );
    }
}
