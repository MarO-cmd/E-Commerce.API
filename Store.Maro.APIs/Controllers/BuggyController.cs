using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Maro.APIs.Errors;
using Store.Maro.Repository.Data.Context;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BuggyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var res = await _context.Brands.FindAsync(100);

            if (res is null) return NotFound(new ApiErrorResponse(404));

            return Ok(res);
        }

        [HttpGet("servererror")]
        public async Task<IActionResult> GetServerRequestError()
        {
            var res = await _context.Brands.FindAsync(100);

            var brand = res.ToString();

            return Ok(brand);
        }
            
        [HttpGet("badrequest")]
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }



        [HttpGet("badrequest/{id}")] //validation error
        public async Task<IActionResult> GetBadRequestError(int id) 
        {
            return BadRequest();
        }

         

        [HttpGet("Unauthorized")] 
        public async Task<IActionResult> GetUnauthorizedError(int id)
        {
            return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
        }


    }
}
