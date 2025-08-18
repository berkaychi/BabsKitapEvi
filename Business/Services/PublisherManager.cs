using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using TS.Result;

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

        public async Task<Result<IEnumerable<PublisherDto>>> GetAllAsync()
        {
            var publishers = await _context.Publishers.ToListAsync();
            var publisherDtos = _mapper.Map<IEnumerable<PublisherDto>>(publishers);

            return Result<IEnumerable<PublisherDto>>.Succeed(publisherDtos);
        }

        public async Task<Result<PublisherDto>> GetByIdAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return Result<PublisherDto>.Failure(404, "Publisher not found.");
            }

            var publisherDto = _mapper.Map<PublisherDto>(publisher);
            return Result<PublisherDto>.Succeed(publisherDto);
        }


        public async Task<Result<PublisherDto>> CreateAsync(CreateAndUpdatePublisherDto createPublisherDto)
        {
            var newPublisher = _mapper.Map<Publisher>(createPublisherDto);

            _context.Add(newPublisher);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<PublisherDto>(newPublisher);

            return Result<PublisherDto>.Succeed(resultDto);

        }


        public async Task<Result<string>> UpdateAsync(int id, CreateAndUpdatePublisherDto updatePublisherDto)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return Result<string>.Failure(404, "Publisher not found.");
            }

            _mapper.Map(updatePublisherDto, publisher);

            publisher.Id = id;

            _context.Update(publisher);
            await _context.SaveChangesAsync();

            return Result<string>.Succeed("Publisher updated successfully.");
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return Result<string>.Failure(404, "Publisher not found.");
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return Result<string>.Succeed("Publisher deleted successfully.");
        }
    }
}