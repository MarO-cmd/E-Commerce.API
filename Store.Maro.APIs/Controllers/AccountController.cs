using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Maro.APIs.Errors;
using Store.Maro.Core.Dtos.Auth;
using Store.Maro.Core.Entities.Identity;
using Store.Maro.Core.Services.Contract;
using System.Security.Claims;

namespace Store.Maro.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(
            IUserAuthService userAuthService,
            UserManager<AppUser> userManager,
            ITokenService tokenService
            )
        {
            _userAuthService = userAuthService;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto LoginDto)
        {
            var user =  await _userAuthService.LoginAsync(LoginDto);
            if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto registerDto)
        {
            var user = await _userAuthService.RegisterAsync(registerDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            return Ok(user);
        }

        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        { 
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user =  await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });

        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> UpdateCurrentUserAdress(Adress? adress)
        {
            if (adress is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var userEmail =  User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(userEmail);
            
            user.Adress = adress;

            return Ok();
        }

    }
}
