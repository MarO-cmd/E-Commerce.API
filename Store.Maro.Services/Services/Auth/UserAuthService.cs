using Microsoft.AspNetCore.Identity;
using Store.Maro.Core.Dtos.Auth;
using Store.Maro.Core.Entities.Identity;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Services.Services.TokenServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Services.Services.Auth
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserAuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
            
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            // check if there is exist such an email
            var user =  await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return null;
            // check password
            var res =  await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if (!res.Succeeded) return null;


            return new UserDto() 
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null) return null;

            var user = new AppUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split('@')[0]
            };

            var result =  await _userManager.CreateAsync(user);
            if (!result.Succeeded) return null;

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }
    }
}
