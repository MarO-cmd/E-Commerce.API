using Store.Maro.Core.Dtos.Product;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Helper;
using Store.Maro.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface IProductService
    {  
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParames productSpec);
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<ProductDto> GetProductById(int id);

    }
}
