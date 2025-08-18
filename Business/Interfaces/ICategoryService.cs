using BabsKitapEvi.Common.DTOs.CategoryDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IServiceResult> GetAllAsync();
        Task<IServiceResult> GetByIdAsync(int id);
        Task<IServiceResult> CreateAsync(CreateAndUpdateCategoryDto createCategoryDto);
        Task<IServiceResult> UpdateAsync(int id, CreateAndUpdateCategoryDto categoryDto);
        Task<IServiceResult> DeleteAsync(int id);
    }
}