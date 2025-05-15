using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Maro.Core.Services.Contract;
using System;
using System.Text;

namespace Store.Maro.APIs.Attributes
{
    // IAsyncActionFilter, meaning it can intercept controller actions before and after they run =>
    // before u enter the endpoint this class is executed first
    public class CachedAttribute : Attribute,IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        // This is the heart of the logic that runs before and after the controller action.
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //It retrieves your custom cache service (ICashService) from the built-in DI container.
            // ASK CLR inject object from ICashService
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICashService>();

            var key = KeyGenerator(context.HttpContext.Request);

            var cachedData = await cacheService.GetCachKeyAsync(key);

            if(!string.IsNullOrEmpty(cachedData))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedData,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            //Execute the action and cache its result

            var executionResult = await next.Invoke();  // execution

            if(executionResult.Result is OkObjectResult response)
            {
                await cacheService.SetCachKeyAsync(key, response.Value, TimeSpan.FromSeconds(_expireTime));
            }

        }

        // Creates a cache key based on the URL path and query string (like page number or filters).
        // api/products?pageIndex=1&pageSize=5 → api/products|pageIndex-1|pageSize-5
        private string KeyGenerator(HttpRequest request)
        {

            var cacheKey = new StringBuilder();

            
            cacheKey.Append($"{request.Path}"); // api/product

            foreach (var (key , value) in request.Query.OrderBy(x => x.Key))
            {
                
                cacheKey.Append($"|{key}-{value}"); // api/product|pageIndex-1|pageSize-5 [pagination]
            }

            return cacheKey.ToString();

        }
    }
}
