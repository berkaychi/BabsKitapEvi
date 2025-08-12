using System.Net.Mime;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using FluentValidation;

namespace BabsKitapEvi.Business.Validators.Book
{
    public class UpdateBookImageDtoValidator : AbstractValidator<UpdateBookImageDto>
    {
        private const int MaxFileSizeInMb = 5;
        private const int MaxFileSizeInBytes = MaxFileSizeInMb * 1024 * 1024;
        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };

        public UpdateBookImageDtoValidator()
        {
            RuleFor(x => x.ImageFile)
                .NotNull()
                .WithMessage("Image file is required.");

            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile.Length)
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                    .WithMessage($"File size cannot exceed {MaxFileSizeInMb}MB.");

                RuleFor(x => x.ImageFile.ContentType)
                    .Must(ContentType => AllowedMimeTypes.Contains(ContentType.ToLower()))
                    .WithMessage($"Invalid file type. Allowed types are: {string.Join(", ", AllowedMimeTypes)}");
            });
        }
    }

}