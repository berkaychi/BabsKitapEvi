using BabsKitapEvi.Business.Validators.Book.BusinessRuleValidators;
using TS.Result;

namespace BabsKitapEvi.Business.Validators.Book
{
    public interface IBookBusinessRuleValidator
    {
        Task<Result<string>> Validate(BookValidatorRequest request);
    }
}