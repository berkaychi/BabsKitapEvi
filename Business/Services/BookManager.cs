using AutoMapper;
using BabsKitapEvi.DataAccess;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Entities.DTOs.BookDTOs;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BabsKitapEvi.Business.Services
{
    public sealed class BookManager : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<BookDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
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


        public async Task<Result<BookDto>> CreateAsync(CreateBookDto createBookDto)
        {
            if (await _context.Books.AnyAsync(b => b.ISBN == createBookDto.ISBN))
            {
                return Result<BookDto>.Failure(400, "A book with this ISBN already exists.");
            }

            if (await _context.Books.AnyAsync(b => b.Title == createBookDto.Title && b.Author == createBookDto.Author))
            {
                return Result<BookDto>.Failure(400, "A book with this title and author already exists.");
            }

            var book = _mapper.Map<Book>(createBookDto);
            book.BookCategories = new List<BookCategory>();

            if (createBookDto.CategoryIds != null && createBookDto.CategoryIds.Any())
            {
                var categories = await _context.Categories.Where(c => createBookDto.CategoryIds.Contains(c.Id)).ToListAsync();
                foreach (var category in categories)
                {
                    book.BookCategories.Add(new BookCategory { Category = category });
                }
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Result<BookDto>.Succeed(_mapper.Map<BookDto>(book));
        }


        public async Task<Result<string>> UpdateAsync(int id, UpdateBookDto updateBookDto)
        {
            var book = await _context.Books
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return Result<string>.Failure(404, "Book not found.");
            }
            if (!string.IsNullOrEmpty(updateBookDto.ISBN) && await _context.Books.AnyAsync(b => b.ISBN == updateBookDto.ISBN && b.Id != id))
            {
                return Result<string>.Failure(400, "A book with this ISBN already exists.");
            }

            _mapper.Map(updateBookDto, book);

            if (updateBookDto.CategoryIds != null)
            {
                book.BookCategories!.Clear();
                var categories = await _context.Categories.Where(c => updateBookDto.CategoryIds.Contains(c.Id)).ToListAsync();
                foreach (var category in categories)
                {
                    book.BookCategories.Add(new BookCategory { Category = category });
                }
            }

            book.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Book updated successfully.");
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return Result<string>.Failure(404, "Book not found.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Result<string>.Succeed("Book deleted successfully.");
        }
    }
}