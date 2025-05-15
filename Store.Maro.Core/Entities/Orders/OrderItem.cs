using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Entities.Orders
{
    public class OrderItem : BaseEntity<int>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrder product, decimal price, int quantity)
        {
            this.product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrder product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
