using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Dtos.Orders
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        // the creation of the order 
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;  // offset cuz it shows u the timezone
        // will brought from db as string as we did it in the config
        public string Status { get; set; } 
        public AddressDto ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
        public decimal SubTotal { get; set; } // price without delivery
        public decimal Total { get; set; }
        public string? PaymentIntentId { get; set; } = string.Empty;
    }
}
