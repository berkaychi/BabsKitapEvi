using AutoMapper;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Common.DTOs.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BabsKitapEvi.Business.Services
{
    public sealed class BookImageManager : IBookImageService
    {
        private readonly IImageUploadService _imageUploadService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public BookImageManager(IImageUploadService imageUploadService, IMapper mapper, ApplicationDbContext context)
        {
            _imageUploadService = imageUploadService;
            _mapper = mapper;
            _context = context;
        }

        public Task<Result<string>> DeleteImageAsync(string? ImagePublicId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<BookImageDto>> UpdateBookImageAsync(int bookId, IFormFile imageFile, CancellationToken ct = default)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return Result<BookImageDto>.Failure(404, "Book not found.");
            }

            if (imageFile == null || imageFile.Length <= 0)
                return Result<BookImageDto>.Failure(400, "Image file is required.");

            var oldImagePublicId = book.ImagePublicId;

            try
            {
                var uploadResult = await UploadImageAsync(imageFile, ct);
                if (!uploadResult.IsSuccessful)
                {
                    return Result<BookImageDto>.Failure(uploadResult.StatusCode, uploadResult.ErrorMessages);
                }

                var updateDto = new BookImageUpdateDto
                {
                    BookId = bookId,
                    ImageUrl = uploadResult.Data.Url,
                    ImagePublicId = uploadResult.Data.PublicId,
                };

                _mapper.Map(updateDto, book);
                book.UpdatedAt = DateTime.UtcNow;

                if (oldImagePublicId != null)
                {
                    await _imageUploadService.DeleteImageAsync(oldImagePublicId, ct);
                }

                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                return Result<BookImageDto>.Failure(500, ex.Message);
            }

            return Result<BookImageDto>.Succeed(new BookImageDto());
        }

        public Task<Result<ImageUploadResultDto>> UploadImageAsync(IFormFile imageFile, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}