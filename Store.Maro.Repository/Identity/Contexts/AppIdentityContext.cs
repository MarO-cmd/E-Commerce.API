using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Maro.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Repository.Identity.Contexts
{
    // i put AppUser here to use my custome class not the default class [IdentityUser]
    public class AppIdentityContext : IdentityDbContext<AppUser>
    {
        // passing the configuration [connection string]
        public AppIdentityContext(DbContextOptions<AppIdentityContext> options ) : base(options)
        {
            
        }
    }
}
