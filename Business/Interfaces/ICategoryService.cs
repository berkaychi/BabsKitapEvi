using BabsKitapEvi.Common.DTOs.CategoryDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IServiceResult<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<IServiceResult<CategoryDto>> GetByIdAsync(int id);
        Task<IServiceResult<CategoryDto>> CreateAsync(CreateAndUpdateCategoryDto createCategoryDto);
        Task<IServiceResult<CategoryDto>> UpdateAsync(int id, CreateAndUpdateCategoryDto categoryDto);
        Task<IServiceResult> DeleteAsync(int id);
    }
}