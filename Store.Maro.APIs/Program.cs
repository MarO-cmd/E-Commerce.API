
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.Maro.APIs.Errors;
using Store.Maro.APIs.Middlewares;
using Store.Maro.Core;
using Store.Maro.Core.Entities.Identity;
using Store.Maro.Core.Mapping.Baskets;
using Store.Maro.Core.Mapping.Orders;
using Store.Maro.Core.Mapping.Products;
using Store.Maro.Core.Repository.Contract;
using Store.Maro.Core.Services.Contract;
using Store.Maro.Repository;
using Store.Maro.Repository.Data;
using Store.Maro.Repository.Data.Context;
using Store.Maro.Repository.Identity.Contexts;
using Store.Maro.Repository.Identity.DateSeeds;
using Store.Maro.Repository.Repositories;
using Store.Maro.Services.Services.Auth;
using Store.Maro.Services.Services.BasketServices;
using Store.Maro.Services.Services.CacheServices;
using Store.Maro.Services.Services.OrderServices;
using Store.Maro.Services.Services.Payments;
using Store.Maro.Services.Services.ProductSercvices;
using Store.Maro.Services.Services.TokenServices;
using System.ComponentModel;
using System.Data;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Store.Maro.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // builder.Services : This is the dependency injection container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                // connection string
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityContext>(option =>
            {
                // connection string
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddScoped<IProductService, ProductSercice>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICashService, CachService>();
            builder.Services.AddScoped<IUserAuthService, UserAuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentServices>();


            #region Explination of AddEntityFrameworkStores
            //Dear Identity, please save all users, roles, logins, and tokens in the database
            //using my context (AppIdentityContext) which already knows how to handle Identity tables.
            //So when you call .AddEntityFrameworkStores<AppIdentityContext>(), Identity will:
            //          Use this context to read / write user and role data
            //          Automatically generate and map tables in the database

            ///If you don’t call .AddEntityFrameworkStores<AppIdentityContext>(),
            ///Identity won’t know where to store the data — and stuff like UserManager.CreateAsync()
            ///will fail because it has no database to work with. 
            /// Summary
            ///AddDbContext<AppIdentityContext>() — tells EF Core how to connect to your database.
            ///.UseSqlServer(...) — says use SQL Server with a specific connection string.
            ///.AddEntityFrameworkStores<AppIdentityContext>() — connects Identity to that database via EF Core.
            #endregion

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityContext>();


            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile(builder.Configuration)));
            builder.Services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new OrderProfile(builder.Configuration)));

            builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddAuthentication(options =>
            {
                // which just means "I am going to use JWT Bearer tokens for authentication.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //You configure how the app should check JWT tokens when users send them.
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //Check that the token was created by a trusted server (the correct Issuer).
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    //Check that the token is meant for your app.
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    //Make sure the token is not expired.
                    ValidateLifetime = true,
                    //Verify that the token’s signature is valid, using a key.
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });
            //ApiBehaviorOptions controls how things like model validation errors are handled.

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    //Extract all model state errors (errors during model binding or validation)
                    //Modelstate is collection of key and  value and its a tree has node(key) and children (values) 
                    var errors = actionContext.ModelState.Where(item => item.Value.Errors.Count() > 0)
                                            .SelectMany(item => item.Value.Errors)  // Since each property might have multiple errors, this flattens them into one list.
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();
                    var response = new ApiValidatoinErrorResponse()
                    {

                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            var app = builder.Build();

            //create a container that has all services that use lifetime scope
            using var scope = app.Services.CreateScope();

            var service = scope.ServiceProvider;
            // injections by CLR
            var context = service.GetService<AppDbContext>();
            var Identitycontext = service.GetService<AppIdentityContext>();
            var userManager = service.GetService<UserManager<AppUser>>();

            var loggerFactory = service.GetService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

                await Identitycontext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "there is a problem while appling the migration");
            }

            //middleware is a component in the pipeline that handles HTTP requests/responses
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
