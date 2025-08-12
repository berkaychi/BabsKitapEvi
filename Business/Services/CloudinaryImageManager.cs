using BabsKitapEvi.Business.Interfaces;
using BabsKitapEvi.Entities.DTOs.Shared;
using BabsKitapEvi.Entities.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace BabsKitapEvi.Business.Services
{
    public sealed class CloudinaryImageManager : IImageUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageManager(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResultDto> UploadImageAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string? folder = null,
            CancellationToken cancellationToken = default)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = string.IsNullOrWhiteSpace(folder) ? "babs-kitap-evi/books" : folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
                Transformation = new Transformation()
                    .Width(300)
                    .Height(450)
                    .Crop("pad")
                    .Gravity("center")
                    .Background("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            return new ImageUploadResultDto
            {
                Url = uploadResult.SecureUrl?.ToString() ?? string.Empty,
                PublicId = uploadResult.PublicId ?? string.Empty,
                Width = uploadResult.Width,
                Height = uploadResult.Height
            };
        }

        public async Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(publicId))
            {
                return;
            }

            var deletionParams = new DeletionParams(publicId)
            {
                Invalidate = true
            };
            await _cloudinary.DestroyAsync(deletionParams);
        }
    }
}