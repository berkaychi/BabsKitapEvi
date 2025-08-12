namespace BabsKitapEvi.Entities.DTOs.BookDTOs
{
    public sealed class BookImageUpdateDto
    {
        public int BookId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;
        public string? OldImagePublicId { get; set; }
    }
}