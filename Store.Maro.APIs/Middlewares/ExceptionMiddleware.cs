using Store.Maro.APIs.Errors;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Store.Maro.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // this execute the next middleware 
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                // This line tells the client (like a browser or a mobile app) that the type of data being returned is JSON.
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;


                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse(StatusCodes.Status500InternalServerError, ex.Message, ex.StackTrace?.ToString())
                    : new ApiExceptionResponse(StatusCodes.Status500InternalServerError);

                var json = JsonSerializer.Serialize(response);


                //This line writes the actual JSON content(which you just prepared) into the HTTP response body and sends it back to the client.
                await context.Response.WriteAsync(json);
            }
        }
    }
}
