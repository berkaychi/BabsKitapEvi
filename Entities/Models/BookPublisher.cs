namespace BabsKitapEvi.Entities.Models
{
    public class BookPublisher
    {
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
    }
}