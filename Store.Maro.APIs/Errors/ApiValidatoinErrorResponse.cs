namespace Store.Maro.APIs.Errors
{
    public class ApiValidatoinErrorResponse : ApiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public ApiValidatoinErrorResponse(): base(StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
