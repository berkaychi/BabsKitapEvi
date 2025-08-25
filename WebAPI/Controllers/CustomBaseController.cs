using BabsKitapEvi.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult(IServiceResult result)
        {
            if (result.IsSuccess)
            {
                var dataProperty = result.GetType().GetProperty("Data");
                if (dataProperty != null)
                {
                    var data = dataProperty.GetValue(result);
                    var response = new
                    {
                        data = data,
                        isSuccess = true,
                        message = result.Message,
                        errors = default(string[]?)
                    };

                    if (result.StatusCode == 201)
                    {
                        var idProperty = data?.GetType().GetProperty("Id");
                        var id = idProperty?.GetValue(data);
                        return CreatedAtAction("GetById", new { id }, response);
                    }

                    return new ObjectResult(response)
                    {
                        StatusCode = result.StatusCode
                    };
                }

                // Data olmayan başarılı durumlar (SuccessResult)
                var successResponse = new
                {
                    data = default(object?),
                    isSuccess = true,
                    message = result.Message,
                    errors = default(string[]?)
                };
                return new ObjectResult(successResponse)
                {
                    StatusCode = result.StatusCode
                };
            }

            // Hata durumları
            var errorResult = result as ErrorResult;
            var errorResponse = new
            {
                data = default(object?),
                isSuccess = false,
                message = result.Message,
                errors = errorResult?.Errors?.ToArray() ?? new[] { result.Message ?? "An error occurred." }
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = result.StatusCode
            };
        }
        [NonAction]
        protected string? GetCurrentUserId()
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    [Authorize]
    public abstract class PrivateBaseController : CustomBaseController
    {
        protected string UserId => GetCurrentUserId() ??
                                    throw new InvalidOperationException("User ID claim not found in token.");
    }
}