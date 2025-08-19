using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IPublisherService
    {
        Task<IServiceResult> GetAllAsync();
        Task<IServiceResult> GetByIdAsync(int id);
        Task<IServiceResult> CreateAsync(CreateAndUpdatePublisherDto createPublisherDto);
        Task<IServiceResult> UpdateAsync(int id, CreateAndUpdatePublisherDto updatePublisherDto);
        Task<IServiceResult> DeleteAsync(int id);
    }
}