using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Maro.APIs.Errors;
using Store.Maro.Core;
using Store.Maro.Core.Dtos.Orders;
using Store.Maro.Core.Entities.Orders;
using Store.Maro.Core.Services.Contract;
using System.Security.Claims;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(
            IOrderService orderService,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));


            var address = _mapper.Map<Address>(model.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(userEmail, model.BasketId, model.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<OrderToReturnDto>(order));

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var orders = await _orderService.GetOrdersForSpecificUserAsync(userEmail);
            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var order = await _orderService.GetOrderByIdForSpecificUserAsync(userEmail, orderId);
            if (order is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<OrderToReturnDto>(order) );
        }

        [HttpGet("GetDeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethods is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(deliveryMethods);

        }

    }
}
