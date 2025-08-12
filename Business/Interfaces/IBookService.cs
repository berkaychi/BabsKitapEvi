using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Common.DTOs.Shared;
using Microsoft.AspNetCore.Http;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IBookService
    {
        Task<Result<IEnumerable<BookDto>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Result<IEnumerable<BookDto>>> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize);
        Task<Result<BookDto>> GetByIdAsync(int id);
        Task<Result<BookDto>> CreateAsync(CreateBookDto createBookDto, string? imageUrl = null, string? imagePublicId = null, CancellationToken ct = default);
        Task<Result<string>> UpdateAsync(int id, UpdateBookDto updateBookDto, string? newImageUrl = null, string? newImagePublicId = null, CancellationToken ct = default);
        Task<Result<string>> UpdateImageAsync(int id, IFormFile imageFile, CancellationToken ct = default);
        Task<Result<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}