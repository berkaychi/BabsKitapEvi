namespace BabsKitapEvi.Entities.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<BookPublisher> BookPublishers { get; set; } = new List<BookPublisher>();
    }
}