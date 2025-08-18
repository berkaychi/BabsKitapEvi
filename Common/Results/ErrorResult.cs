namespace BabsKitapEvi.Common.Results
{
    public class ErrorResult : IServiceResult
    {
        public bool IsSuccess => false;
        public string Message { get; }
        public int StatusCode { get; }
        public List<string> Errors { get; }

        public ErrorResult(int statusCode, string message, List<string> errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors ?? new List<string> { message };
        }

        public ErrorResult(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            Message = errors.FirstOrDefault() ?? "An error occurred.";
            Errors = errors;
        }
    }
}