namespace BabsKitapEvi.Common.Results
{
    public class SuccessResult : IServiceResult
    {
        public bool IsSuccess => true;
        public string Message { get; }
        public int StatusCode { get; }

        public SuccessResult(int statusCode, string message = "Operation successful.")
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}