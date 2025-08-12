namespace BabsKitapEvi.Common.DTOs.Shared
{
    public sealed class ImageUploadResultDto
    {
        public string Url { get; init; } = string.Empty;
        public string PublicId { get; init; } = string.Empty;
        public int? Width { get; init; }
        public int? Height { get; init; }
    }
}


