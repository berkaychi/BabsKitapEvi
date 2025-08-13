using System.IO.Compression;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using FluentValidation;

namespace BabsKitapEvi.Business.Validators.Book
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        private const int MaxFileSizeInMb = 5;
        private const int MaxFileSizeInBytes = MaxFileSizeInMb * 1024 * 1024;
        private const int MaxTitleLength = 50;
        private const int MaxAuthorNameLength = 50;
        private const int MaxDescriptionLength = 1000;
        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };

        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(MaxTitleLength).WithMessage($"Title must be at most {MaxTitleLength} characters long.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author name is required.")
                .MaximumLength(MaxAuthorNameLength).WithMessage($"Author name must be at most {MaxAuthorNameLength} characters long.");

            RuleFor(x => x.Description)
                .MaximumLength(MaxDescriptionLength).WithMessage($"Description must be at most {MaxDescriptionLength} characters long.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN alanı boş olamaz.")
                .Length(10, 13).WithMessage("ISBN 10 veya 13 karakter olmalıdır.");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Yayınlanma tarihi gelecek bir tarih olamaz.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");

            RuleFor(x => x.Price)
               .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("En az bir kategori seçilmelidir.");

            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile!.Length)
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                    .WithMessage($"Resim boyutu {MaxFileSizeInMb}MB'tan büyük olamaz.");

                RuleFor(x => x.ImageFile!.ContentType)
                    .Must(x => AllowedMimeTypes.Contains(x))
                    .WithMessage($"Sadece {string.Join(", ", AllowedMimeTypes)} formatında resim yükleyebilirsiniz.");
            });
        }

    }
}