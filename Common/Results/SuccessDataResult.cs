namespace BabsKitapEvi.Common.Results
{
    public class SuccessDataResult<T> : SuccessResult, IServiceResult<T>
    {
        public T Data { get; }

        public SuccessDataResult(T data, int statusCode, string message = "Operation successful.") : base(statusCode, message)
        {
            Data = data;
        }
    }
}