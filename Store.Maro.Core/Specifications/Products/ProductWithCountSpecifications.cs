﻿using Store.Maro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Specifications.Products
{
    public class ProductWithCountSpecifications :BaseSpecifications<Product , int>
    {
        public ProductWithCountSpecifications(ProductSpecParames productSpec) :
            base(
                     P =>
                     (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                     &&
                     (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId)
                     && (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)
                 )
        {

        }
    }
}
