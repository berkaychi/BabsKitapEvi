using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Common.DTOs.Shared;
using Microsoft.AspNetCore.Http;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IBookService
    {
        Task<IServiceResult> GetAllAsync(int pageNumber, int pageSize);
        Task<IServiceResult> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize);
        Task<IServiceResult> GetByPublisherIdAsync(int publisherId, int pageNumber, int pageSize);
        Task<IServiceResult> GetByIdAsync(int id);
        Task<IServiceResult> CreateAsync(CreateBookDto createBookDto, string? imageUrl = null, string? imagePublicId = null, CancellationToken ct = default);
        Task<IServiceResult> UpdateAsync(int id, UpdateBookDto updateBookDto, CancellationToken ct = default);
        Task<IServiceResult> UpdateImageAsync(int id, IFormFile imageFile, CancellationToken ct = default);
        Task<IServiceResult> DeleteAsync(int id, CancellationToken ct = default);
        Task<IServiceResult> SearchAsync(BooksQuery query, CancellationToken ct = default);
    }
}