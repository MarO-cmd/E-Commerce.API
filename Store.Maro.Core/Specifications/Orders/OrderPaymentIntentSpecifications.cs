using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Specifications.Orders
{
    public class OrderPaymentIntentSpecifications :BaseSpecifications<Order,int>
    {
        public OrderPaymentIntentSpecifications(string paymentIntentId) : base(
           O => O.PaymentIntentId == paymentIntentId
           )
        {
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
