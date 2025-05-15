using Microsoft.AspNetCore.Identity;
using Store.Maro.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Services.Contract
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> _userManager);
    }
}
