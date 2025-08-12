using AutoMapper;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using Microsoft.EntityFrameworkCore;
using TS.Result;
using Microsoft.AspNetCore.Http;

namespace BabsKitapEvi.Business.Services
{
    public sealed class BookManager : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageUploadService _imageUploadService;

        public BookManager(ApplicationDbContext context, IMapper mapper, IImageUploadService imageUploadService)
        {
            _context = context;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
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
            if (await _context.Books.AnyAsync(b => b.ISBN == createBookDto.ISBN, ct))
            {
                return Result<BookDto>.Failure(400, "A book with this ISBN already exists.");
            }

            if (await _context.Books.AnyAsync(b => b.Title == createBookDto.Title && b.Author == createBookDto.Author, ct))
            {
                return Result<BookDto>.Failure(400, "A book with this title and author already exists.");
            }

            var book = _mapper.Map<Book>(createBookDto);
            book.ImageUrl = imageUrl;
            book.ImagePublicId = imagePublicId;
            book.BookCategories = new List<BookCategory>();

            if (createBookDto.CategoryIds != null && createBookDto.CategoryIds.Any())
            {
                var categories = await _context.Categories.Where(c => createBookDto.CategoryIds.Contains(c.Id)).ToListAsync(ct);
                foreach (var category in categories)
                {
                    book.BookCategories.Add(new BookCategory { Category = category });
                }
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync(ct);
            return Result<BookDto>.Succeed(_mapper.Map<BookDto>(book));
        }


        public async Task<Result<string>> UpdateAsync(int id, UpdateBookDto updateBookDto, string? newImageUrl = null, string? newImagePublicId = null, CancellationToken ct = default)
        {
            var book = await _context.Books
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.Id == id, ct);

            if (book == null)
            {
                return Result<string>.Failure(404, "Book not found.");
            }
            if (!string.IsNullOrEmpty(updateBookDto.ISBN) && await _context.Books.AnyAsync(b => b.ISBN == updateBookDto.ISBN && b.Id != id, ct))
            {
                return Result<string>.Failure(400, "A book with this ISBN already exists.");
            }

            _mapper.Map(updateBookDto, book);

            if (!string.IsNullOrWhiteSpace(newImageUrl) && !string.IsNullOrWhiteSpace(newImagePublicId))
            {
                // UI/Controller eski görseli silme kararını dışarıda verecek; burada sadece güncelle
                book.ImageUrl = newImageUrl;
                book.ImagePublicId = newImagePublicId;
            }

            if (updateBookDto.CategoryIds != null)
            {
                book.BookCategories!.Clear();
                var categories = await _context.Categories.Where(c => updateBookDto.CategoryIds.Contains(c.Id)).ToListAsync(ct);
                foreach (var category in categories)
                {
                    book.BookCategories.Add(new BookCategory { Category = category });
                }
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

            try
            {
                string? imageUrl = null;
                string? imagePublicId = null;

                using var stream = imageFile.OpenReadStream();
                var upload = await _imageUploadService.UploadImageAsync(
                    stream,
                    imageFile.FileName,
                    imageFile.ContentType,
                    "babs-kitap-evi/books",
                    ct
                );
                imageUrl = upload.Url;
                imagePublicId = upload.PublicId;

                book.ImageUrl = imageUrl;
                book.ImagePublicId = imagePublicId;
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
    }
}