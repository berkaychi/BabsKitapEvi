using BabsKitapEvi.Entities.DTOs.CategoryDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<Result<CategoryDto>> GetByIdAsync(int id);
        Task<Result<CategoryDto>> CreateAsync(CategoryDto categoryDto);
        Task<Result<string>> UpdateAsync(int id, CategoryDto categoryDto);
        Task<Result<string>> DeleteAsync(int id);
    }
}