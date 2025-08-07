using System.Collections.Generic;

namespace BabsKitapEvi.Entities.DTOs.BookDTOs
{
    public sealed class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? StockQuantity { get; set; }
        public decimal? Price { get; set; }
        public List<int>? CategoryIds { get; set; }
    }
}