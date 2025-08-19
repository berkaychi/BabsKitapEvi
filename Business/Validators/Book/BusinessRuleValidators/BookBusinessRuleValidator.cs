using BabsKitapEvi.DataAccess;
using Microsoft.EntityFrameworkCore;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Validators.Book.BusinessRuleValidators
{
    public class BookBusinessRuleValidator : IBookBusinessRuleValidator
    {
        private readonly ApplicationDbContext _context;

        public BookBusinessRuleValidator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IServiceResult> Validate(BookValidatorRequest request)
        {
            var isbnQuery = _context.Books.Where(b => b.ISBN == request.ISBN);
            if (request.Id.HasValue)
            {
                isbnQuery = isbnQuery.Where(b => b.Id != request.Id);
            }

            if (await isbnQuery.AnyAsync())
            {
                return new ErrorResult(400, "A book with the same ISBN already exists.");
            }

            var titleAuthorQuery = _context.Books.Where(b => b.Title == request.Title && b.Author == request.Author);
            if (request.Id.HasValue)
            {
                titleAuthorQuery = titleAuthorQuery.Where(b => b.Id != request.Id.Value);
            }
            if (await titleAuthorQuery.AnyAsync())
            {
                return new ErrorResult(400, "A book with the same title and author already exists.");
            }

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                var distinctIdCount = request.CategoryIds.Distinct().Count();

                var matchingCountInDb = await _context.Categories
                    .CountAsync(c => request.CategoryIds.Contains(c.Id));

                if (distinctIdCount != matchingCountInDb)
                {
                    return new ErrorResult(400, "Some category IDs do not exist in the database.");
                }
            }

            return new SuccessResult(200, "Book validation passed.");
        }
    }
}