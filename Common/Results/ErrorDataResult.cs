namespace BabsKitapEvi.Common.Results
{
    public class ErrorDataResult<T> : ErrorResult
    {
        public T Data { get; }

        public ErrorDataResult(T data, int statusCode, string message)
            : base(statusCode, message)
        {
            Data = data;
        }

        public ErrorDataResult(T data, int statusCode, List<string> errors) : base(statusCode, errors)
        {
            Data = data;
        }
    }
}