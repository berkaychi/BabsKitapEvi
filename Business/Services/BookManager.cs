using AutoMapper;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using Microsoft.EntityFrameworkCore;
using TS.Result;
using Microsoft.AspNetCore.Http;
using BabsKitapEvi.Business.Validators.Book;
using BabsKitapEvi.Common.DTOs.Shared;
using System.Data.Common;
using BabsKitapEvi.Business.Extensions;
using AutoMapper.QueryableExtensions;

namespace BabsKitapEvi.Business.Services
{
    public sealed class BookManager : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageUploadService _imageUploadService;
        private readonly IBookBusinessRuleValidator _bookBusinessRuleValidator;

        public BookManager(ApplicationDbContext context, IMapper mapper, IImageUploadService imageUploadService, IBookBusinessRuleValidator bookBusinessRuleValidator)
        {
            _context = context;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
            _bookBusinessRuleValidator = bookBusinessRuleValidator;
        }

        public async Task<Result<IEnumerable<BookDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return Result<IEnumerable<BookDto>>.Failure(400, "Page number and page size must be greater than zero.");
            }
            var books = await _context.Books
            .AsNoTracking()
            .Include(b => b.BookCategories!)
                .ThenInclude(bc => bc.Category)
            .OrderBy(b => b.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return Result<IEnumerable<BookDto>>.Succeed(_mapper.Map<IEnumerable<BookDto>>(books));
        }
        public async Task<Result<IEnumerable<BookDto>>> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize)
        {
            if (await _context.Categories.AnyAsync(c => c.Id == categoryId) == false)
            {
                return Result<IEnumerable<BookDto>>.Failure(404, "Category not found.");
            }

            if (pageNumber < 1 || pageSize < 1)
            {
                return Result<IEnumerable<BookDto>>.Failure(400, "Page number and page size must be greater than zero.");
            }

            var books = await _context.Books
                .AsNoTracking()
                .Include(b => b.BookCategories!)
                    .ThenInclude(bc => bc.Category)
                .Where(b => b.BookCategories!.Any(bc => bc.CategoryId == categoryId))
                .OrderBy(b => b.Title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Result<IEnumerable<BookDto>>.Succeed(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        public async Task<Result<IEnumerable<BookDto>>> GetByPublisherIdAsync(int publisherId, int pageNumber, int pageSize)
        {
            if (await _context.Publishers.AnyAsync(p => p.Id == publisherId) == false)
            {
                return Result<IEnumerable<BookDto>>.Failure(404, "Publisher not found.");
            }

            if (pageNumber < 1 || pageSize < 1)
            {
                return Result<IEnumerable<BookDto>>.Failure(400, "Page number and page size must be greater than zero.");
            }

            var books = await _context.Books
               .AsNoTracking()
               .Include(b => b.BookPublishers!)
                   .ThenInclude(bc => bc.Publisher)
               .Where(b => b.BookPublishers!.Any(bc => bc.PublisherId == publisherId))
               .OrderBy(b => b.Title)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return Result<IEnumerable<BookDto>>.Succeed(_mapper.Map<IEnumerable<BookDto>>(books));
        }
        public async Task<Result<BookDto>> GetByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookCategories!)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return Result<BookDto>.Failure(404, "Book not found.");
            }

            return Result<BookDto>.Succeed(_mapper.Map<BookDto>(book));
        }


        public async Task<Result<BookDto>> CreateAsync(CreateBookDto createBookDto, string? imageUrl = null, string? imagePublicId = null, CancellationToken ct = default)
        {
            var validationResult = await _bookBusinessRuleValidator.Validate(createBookDto);
            if (!validationResult.IsSuccessful)
            {
                return Result<BookDto>.Failure(validationResult.StatusCode, validationResult.ErrorMessages);
            }

            var book = _mapper.Map<Book>(createBookDto);
            book.ImageUrl = imageUrl;
            book.ImagePublicId = imagePublicId;
            book.BookCategories = new List<BookCategory>();

            if (createBookDto.CategoryIds != null && createBookDto.CategoryIds.Any())
            {
                await AddCategoriesToNewBookAsync(book, createBookDto.CategoryIds, ct);
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync(ct);
            return Result<BookDto>.Succeed(_mapper.Map<BookDto>(book));
        }

        public async Task<Result<string>> UpdateAsync(int id, UpdateBookDto updateBookDto, CancellationToken ct = default)
        {
            var book = await _context.Books
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.Id == id, ct);

            if (book == null)
            {
                return Result<string>.Failure(404, "Book not found.");
            }

            updateBookDto.Id = id;
            var validationResult = await _bookBusinessRuleValidator.Validate(updateBookDto);
            if (!validationResult.IsSuccessful)
            {
                return Result<string>.Failure(validationResult.StatusCode, validationResult.ErrorMessages);
            }

            _mapper.Map(updateBookDto, book);

            if (updateBookDto.CategoryIds != null && updateBookDto.CategoryIds.Any())
            {
                await AddCategoriesToNewBookAsync(book, updateBookDto.CategoryIds, ct);
            }

            book.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return Result<string>.Succeed("Book updated successfully.");
        }

        public async Task<Result<string>> UpdateImageAsync(int id, IFormFile imageFile, CancellationToken ct = default)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id, ct);
            if (book == null)
            {
                return Result<string>.Failure(404, "Book not found.");
            }

            if (imageFile == null || imageFile.Length == 0)
            {
                return Result<string>.Failure(400, "Image file is required.");
            }

            string? oldImagePublicId = book.ImagePublicId;
            string? newImagePublicId = null;

            try
            {
                using var stream = imageFile.OpenReadStream();
                var uploadResult = await _imageUploadService.UploadImageAsync(
                    stream,
                    imageFile.FileName,
                    imageFile.ContentType,
                    "babs-kitap-evi/books",
                    ct);

                newImagePublicId = uploadResult.PublicId;

                book.ImageUrl = uploadResult.Url;
                book.ImagePublicId = uploadResult.PublicId;
                book.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(ct);

                if (!string.IsNullOrWhiteSpace(oldImagePublicId))
                {
                    try
                    {
                        await _imageUploadService.DeleteImageAsync(oldImagePublicId, ct);
                    }
                    catch
                    {
                        // Buraya loglama eklenecek.
                    }
                }

                return Result<string>.Succeed("Image updated successfully.");
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(newImagePublicId))
                {
                    try
                    {
                        await _imageUploadService.DeleteImageAsync(newImagePublicId, ct);
                    }
                    catch (Exception)
                    {
                        // Buraya da loglama eklenecek.
                    }
                }
                return Result<string>.Failure(500, $"Image update failed: {ex.Message}");
            }
        }

        public async Task<Result<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var book = await _context.Books.FindAsync(new object?[] { id }, ct);
            if (book == null)
            {
                return Result<string>.Failure(404, "Book not found.");
            }

            var imagePublicId = book.ImagePublicId;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync(ct);
            if (!string.IsNullOrWhiteSpace(imagePublicId))
            {
                await _imageUploadService.DeleteImageAsync(imagePublicId!, ct);
            }
            return Result<string>.Succeed("Book deleted successfully.");
        }

        public async Task<Result<PageResult<BookDto>>> SearchAsync(BooksQuery query, CancellationToken ct = default)
        {
            var result = await _context.Books
                .AsNoTracking()
                .ApplyFilters(query)
                .ApplySorting(query.SortBy, query.SortDirection)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToPageResultAsync(query.PageNumber, query.PageSize, ct);

            return Result<PageResult<BookDto>>.Succeed(result);
        }

        private async Task AddCategoriesToNewBookAsync(Book book, List<int> categoryIds, CancellationToken ct)
        {
            book.BookCategories = new List<BookCategory>();
            if (categoryIds != null && categoryIds.Any())
            {
                var categories = await _context.Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .ToListAsync(ct);

                foreach (var category in categories)
                {
                    book.BookCategories.Add(new BookCategory { Category = category });
                }
            }
        }

    }
}