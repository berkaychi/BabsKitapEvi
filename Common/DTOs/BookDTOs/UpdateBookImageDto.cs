using Microsoft.AspNetCore.Http;

namespace BabsKitapEvi.Common.DTOs.BookDTOs
{
    public sealed class UpdateBookImageDto
    {
        public IFormFile ImageFile { get; set; } = null!;
    }
}