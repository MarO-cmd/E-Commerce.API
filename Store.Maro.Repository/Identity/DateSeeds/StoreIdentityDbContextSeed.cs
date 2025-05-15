using Microsoft.AspNetCore.Identity;
using Store.Maro.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Repository.Identity.DateSeeds
{
    public static class StoreIdentityDbContextSeed
    {
        // UserManager It helps you manage users in your application
        public async static Task SeedAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "maroasd33@gmail.com",
                    DisplayName = "MarO",
                    UserName = "MarcellinoAdel",
                    PhoneNumber = "01064093441",
                    
                    Adress = new Adress()
                    {
                        City = "utopia",
                    }
                    
                };

                var res = await _userManager.CreateAsync(user, "P@ssw0rd123!");

                //if(res.Succeeded)
                //{
                //    await Console.Out.WriteLineAsync("yes");
                //}
            }
        }
    }
}
