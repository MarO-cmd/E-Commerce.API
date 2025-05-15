using Store.Maro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Specifications.Products
{
    public class ProductSpecifications : BaseSpecifications<Product, int>
    {

        // ctor for (criteria)
        public ProductSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }


        public ProductSpecifications(ProductSpecParames productSpec) : base(
        P =>
        (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
        &&
        (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId) 
        && 
        (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)
        )
        {
            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {

                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

            ApplyPagination(productSpec.PageSize, productSpec.PageSize * (productSpec.PageIndex - 1));

            ApplyIncludes();
        }

        // add nav property to the list
        private void ApplyIncludes()
        {
            Includes.Add(P => P.Type);
            Includes.Add(P => P.Brand);
        }
    }
}
