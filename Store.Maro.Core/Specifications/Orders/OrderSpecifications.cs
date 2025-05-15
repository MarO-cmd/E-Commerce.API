using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order, int>
    {

        public OrderSpecifications(string buyerEmail, int OrderId) : base(
            O => O.BuyerEmail == buyerEmail && O.Id == OrderId
            )
        {
            ApplyIncludes();

        }
        public OrderSpecifications(string buyerEmail) : base(
            O => O.BuyerEmail == buyerEmail
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
