using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Entities
{
    public class CustomerBasket
    {
        // this stored in redis [InMemory] as key and value 

        // key 
        public string Id { get; set; }

        // value
        public List<CustomerItem> Items { get; set; }

        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

    }
}
