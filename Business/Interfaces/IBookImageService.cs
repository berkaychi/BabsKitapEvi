using BabsKitapEvi.Entities.DTOs.BookDTOs;
using BabsKitapEvi.Entities.DTOs.Shared;
using Microsoft.AspNetCore.Http;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IBookImageService
    {
        Task<Result<BookImageDto>> UpdateBookImageAsync(int bookId, IFormFile imageFile, CancellationToken ct = default);
        Task<Result<string>> DeleteImageAsync(string PublicId, CancellationToken ct = default);
        Task<Result<ImageUploadResultDto>> UploadImageAsync(IFormFile imageFile, CancellationToken ct = default);
    }
}