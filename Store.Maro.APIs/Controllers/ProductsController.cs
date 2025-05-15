using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Store.Maro.APIs.Attributes;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Core.Specifications.Products;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Cached(60)] // Cache response for 60 seconds
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductSpecParames productSpec)
        {
            var result = await _productService.GetAllProductsAsync(productSpec);

            return Ok(result);
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();

            return Ok(result);
        }

        [HttpGet("Types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("Invalid Id ! ");
            var result = await _productService.GetProductById(id.Value);

            if (result is null) return NotFound($"the product with id {id} not found in DB");

            return Ok(result);
        }

    }
}
