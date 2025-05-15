using Store.Maro.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface IUserAuthService
    {
        public Task<UserDto> LoginAsync(UserLoginDto loginDto);
        public Task<UserDto> RegisterAsync(UserRegisterDto registerDto);
    }
}
