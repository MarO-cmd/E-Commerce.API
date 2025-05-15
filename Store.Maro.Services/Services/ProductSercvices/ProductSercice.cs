using AutoMapper;
using Store.Maro.Core;
using Store.Maro.Core.Dtos.Product;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Helper;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Services.Services.ProductSercvices
{
    public class ProductSercice : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductSercice(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParames productSpec)
        {
            var spec = new ProductSpecifications(productSpec);

            var res = _mapper.Map<IEnumerable<ProductDto>>(await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec));

            // i want to only count the items with specific criteria so i made new class has only criteria
            // we dont need the whole filtration to it will be overKill + when u use pageSize and pageIndex it will incorrect Count
            // why ? it give u right answer if the page size is bigger than the result Otherwise give the page size 
            var countSpec = new ProductWithCountSpecifications(productSpec);

            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);

            return new PaginationResponse<ProductDto>(productSpec.PageIndex,productSpec.PageSize, count ,res) ;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var spec = new ProductSpecifications(id);
            return _mapper.Map<ProductDto>(await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec));
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
        {
            return _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand,int>().GetAllAsync());
        }

     
        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
        {
            return _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
        }

       
    }
}
