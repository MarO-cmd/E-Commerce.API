using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Maro.APIs.Errors;
using Store.Maro.Core.Dtos.Baskets;
using Store.Maro.Core.Services.Contract;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreatePaymentIntent(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var customerBasketDto = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

            if (customerBasketDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(customerBasketDto);
        }
    }
}
