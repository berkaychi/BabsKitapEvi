using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BabsKitapEvi.Common.DTOs.BookDTOs
{
    public sealed class UpdateBookDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? Description { get; set; }
        public int? StockQuantity { get; set; }
        public decimal? Price { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? PublisherIds { get; set; }
    }
}