using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Dtos.Orders
{
    public class OrderDto
    {
        
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }

        public AddressDto ShipToAddress{ get; set; }

    }
}
