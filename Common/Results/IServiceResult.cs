namespace BabsKitapEvi.Common.Results
{
    public interface IServiceResult
    {
        bool IsSuccess { get; }
        string? Message { get; }
        int StatusCode { get; }
    }

    public interface IServiceResult<T> : IServiceResult
    {
        T Data { get; }
    }
}