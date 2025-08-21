using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IPublisherService
    {
        Task<IServiceResult<IEnumerable<PublisherDto>>> GetAllAsync();
        Task<IServiceResult<PublisherDto>> GetByIdAsync(int id);
        Task<IServiceResult<PublisherDto>> CreateAsync(CreateAndUpdatePublisherDto createPublisherDto);
        Task<IServiceResult<PublisherDto>> UpdateAsync(int id, CreateAndUpdatePublisherDto updatePublisherDto);
        Task<IServiceResult> DeleteAsync(int id);
    }
}