using BabsKitapEvi.Common.DTOs.BookDTOs;
using FluentValidation;

namespace BabsKitapEvi.Business.Validators.Book
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Author)
                .MaximumLength(100).WithMessage("Yazar adı en fazla 100 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Author));

            RuleFor(x => x.ISBN)
                .Length(10, 13).WithMessage("ISBN 10 veya 13 karakter olmalıdır.")
                .When(x => !string.IsNullOrEmpty(x.ISBN));

            RuleFor(x => x.PublishedDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Yayınlanma tarihi gelecek bir tarih olamaz.")
                .When(x => x.PublishedDate.HasValue);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.")
                .When(x => x.StockQuantity.HasValue);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.")
                .When(x => x.Price.HasValue);
        }
    }
}