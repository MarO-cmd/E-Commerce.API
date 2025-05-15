using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Entities.Orders
{
    public class Order :BaseEntity<int>
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        // the creation of the order 
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;  // offset cuz it shows u the timezone
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }  //FK
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public decimal SubTotal { get; set; } // price without delivery
        public decimal Total() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }


    }
}
