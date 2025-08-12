using System.IO;
using System.Threading;
using BabsKitapEvi.Entities.DTOs.Shared;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IImageUploadService
    {
        Task<ImageUploadResultDto> UploadImageAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string? folder = null,
            CancellationToken cancellationToken = default);

        Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
    }
}