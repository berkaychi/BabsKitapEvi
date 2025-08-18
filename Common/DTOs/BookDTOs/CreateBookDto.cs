using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BabsKitapEvi.Common.DTOs.BookDTOs
{
    public sealed class CreateBookDto
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; } = 0;
        public decimal Price { get; set; } = 0.0m;
        public List<int>? CategoryIds { get; set; }
        public List<int>? PublisherIds { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}