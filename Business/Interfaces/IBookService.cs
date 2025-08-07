using BabsKitapEvi.Entities.DTOs.BookDTOs;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IBookService
    {
        Task<Result<IEnumerable<BookDto>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Result<BookDto>> GetByIdAsync(int id);
        Task<Result<BookDto>> CreateAsync(CreateBookDto createBookDto);
        Task<Result<string>> UpdateAsync(int id, UpdateBookDto updateBookDto);
        Task<Result<string>> DeleteAsync(int id);
    }
}