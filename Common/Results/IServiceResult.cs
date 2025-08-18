namespace BabsKitapEvi.Common.Results
{
    public interface IServiceResult
    {
        bool IsSuccess { get; }
        string? Message { get; }
        int StatusCode { get; }
    }
}