using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Entities
{
    public class Product :BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }


        // type
        public int? TypeId { get; set; } //FK
        public ProductType  Type { get; set; }


        // brand
        public int? BrandId { get; set; } //FK
        public ProductBrand Brand { get; set; }
    }
}
