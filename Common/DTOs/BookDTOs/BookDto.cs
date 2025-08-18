using BabsKitapEvi.Common.DTOs.CategoryDTOs;
using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using System.Collections.Generic;
namespace BabsKitapEvi.Common.DTOs.BookDTOs
{
    public sealed class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<CategoryDto>? Categories { get; set; }
        public ICollection<PublisherDto>? Publishers { get; set; }
    }
}