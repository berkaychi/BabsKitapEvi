using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using TS.Result;

namespace BabsKitapEvi.Business.Interfaces
{
    public interface IPublisherService
    {
        Task<Result<IEnumerable<PublisherDto>>> GetAllAsync();
        Task<Result<PublisherDto>> GetByIdAsync(int id);
        Task<Result<PublisherDto>> CreateAsync(CreateAndUpdatePublisherDto createPublisherDto);
        Task<Result<string>> UpdateAsync(int id, CreateAndUpdatePublisherDto updatePublisherDto);
        Task<Result<string>> DeleteAsync(int id);
    }
}