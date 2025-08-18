using BabsKitapEvi.Common.DTOs.BookDTOs;

namespace BabsKitapEvi.Business.Validators.Book.BusinessRuleValidators
{
    public class BookValidatorRequest
    {
        public int? Id { get; set; }
        public string? ISBN { get; set; } = null!;
        public string? Title { get; set; } = null!;
        public string? Author { get; set; } = null!;
        public IEnumerable<int>? CategoryIds { get; set; }

        public static implicit operator BookValidatorRequest(CreateBookDto dto)
        {
            return new BookValidatorRequest
            {
                ISBN = dto.ISBN,
                Title = dto.Title,
                Author = dto.Author,
                CategoryIds = dto.CategoryIds
            };
        }

        public static implicit operator BookValidatorRequest(UpdateBookDto dto)
        {
            return new BookValidatorRequest
            {
                Id = dto.Id,
                ISBN = dto.ISBN,
                Title = dto.Title,
                Author = dto.Author,
                CategoryIds = dto.CategoryIds
            };
        }
    }
}
