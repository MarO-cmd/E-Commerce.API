using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Store.Maro.APIs.Errors;
using Store.Maro.Core.Dtos.Baskets;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Services.Services.BasketServices;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

      

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string? id )
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid id "));

            var basket = await _basketService.GetBasketAsync(id);

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateBasket(CustomerBasketDto? model )
        {
            if (model is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var basket = await _basketService.SetBasketAsync(model);

            return Ok(basket);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string? Id)
        {
            if (Id is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Invalid Id !!"));

            await _basketService.DeleteBasketAsync(Id);

            return Ok();
        }


    }
}
