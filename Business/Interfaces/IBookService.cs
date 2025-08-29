using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Common.DTOs.Shared;
using Microsoft.AspNetCore.Http;
using BabsKitapEvi.Common.Results;
using BabsKitapEvi.Common.DTOs.CategoryDTOs;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IBookService
    {
        Task<IServiceResult<PageResult<BookDto>>> GetAllAsync(int pageNumber, int pageSize);
        Task<IServiceResult<PageResult<BookDto>>> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize);
        Task<IServiceResult<PageResult<BookDto>>> GetByPublisherIdAsync(int publisherId, int pageNumber, int pageSize);
        Task<IServiceResult<BookDto>> GetByIdAsync(int id);
        Task<IServiceResult<BookDto>> GetBySlugAsync(string slug);
        Task<IServiceResult<BookDto>> CreateAsync(CreateBookDto createBookDto, string? imageUrl = null, string? imagePublicId = null, CancellationToken ct = default);
        Task<IServiceResult<BookDto>> UpdateAsync(int id, UpdateBookDto updateBookDto, CancellationToken ct = default);
        Task<IServiceResult<BookDto>> UpdateImageAsync(int id, IFormFile imageFile, CancellationToken ct = default);
        Task<IServiceResult<BookDto>> UpdateBookPublisherAsync(int id, UpdateBookPublisherDto updatePublisherDto, CancellationToken ct = default);
        Task<IServiceResult<BookDto>> UpdateBookCategoryAsync(int id, UpdateBookCategoryDto updateCategoryDto, CancellationToken ct = default);
        Task<IServiceResult> DeleteAsync(int id, CancellationToken ct = default);
        Task<IServiceResult<PageResult<BookDto>>> SearchAsync(BooksQuery query, CancellationToken ct = default);
    }
}