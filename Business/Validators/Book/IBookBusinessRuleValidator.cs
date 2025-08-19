using BabsKitapEvi.Business.Validators.Book.BusinessRuleValidators;
using BabsKitapEvi.Common.Results;

namespace BabsKitapEvi.Business.Validators.Book
{
    public interface IBookBusinessRuleValidator
    {
        Task<IServiceResult> Validate(BookValidatorRequest request);
    }
}