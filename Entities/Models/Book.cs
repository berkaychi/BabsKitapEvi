namespace BabsKitapEvi.Entities.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public string? Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<BookCategory>? BookCategories { get; set; }
    }
}