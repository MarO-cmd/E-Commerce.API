using System.Security.Principal;

namespace Store.Maro.APIs.Errors
{
    public class ApiErrorResponse
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null)
        {
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            StatusCode = statusCode;
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            string? message = statusCode switch
            {
                404 => "Not Found",
                400 => "Bad Request",
                401 => "UnAuthourized",
                500 => "Server Error",
                _   => null
            };


            return message;
        }
    }
}
