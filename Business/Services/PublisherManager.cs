using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Services
{
    public sealed class PublisherManager : IPublisherService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PublisherManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IServiceResult<IEnumerable<PublisherDto>>> GetAllAsync()
        {
            var publishers = await _context.Publishers.ToListAsync();
            var publisherDtos = _mapper.Map<IEnumerable<PublisherDto>>(publishers);

            return new SuccessDataResult<IEnumerable<PublisherDto>>(publisherDtos, 200, "Publishers retrieved successfully.");
        }

        public async Task<IServiceResult<PublisherDto>> GetByIdAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return new ErrorDataResult<PublisherDto>(default!, 404, "Publisher not found.");
            }

            var publisherDto = _mapper.Map<PublisherDto>(publisher);
            return new SuccessDataResult<PublisherDto>(publisherDto, 200, "Publisher retrieved successfully.");
        }


        public async Task<IServiceResult<PublisherDto>> CreateAsync(CreateAndUpdatePublisherDto createPublisherDto)
        {
            var newPublisher = _mapper.Map<Publisher>(createPublisherDto);

            _context.Add(newPublisher);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<PublisherDto>(newPublisher);

            return new SuccessDataResult<PublisherDto>(resultDto, 201, "Publisher created successfully.");

        }


        public async Task<IServiceResult<PublisherDto>> UpdateAsync(int id, CreateAndUpdatePublisherDto updatePublisherDto)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return new ErrorDataResult<PublisherDto>(default!, 404, "Publisher not found.");
            }

            _mapper.Map(updatePublisherDto, publisher);

            publisher.Id = id;

            _context.Update(publisher);
            await _context.SaveChangesAsync();

            var updatedPublisherDto = _mapper.Map<PublisherDto>(publisher);
            return new SuccessDataResult<PublisherDto>(updatedPublisherDto, 200, "Publisher updated successfully.");
        }

        public async Task<IServiceResult> DeleteAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return new ErrorResult(404, "Publisher not found.");
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return new SuccessResult(200, "Publisher deleted successfully.");
        }
    }
}