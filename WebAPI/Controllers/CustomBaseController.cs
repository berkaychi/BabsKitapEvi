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
                    if (result.StatusCode == 201)
                    {
                        var idProperty = data?.GetType().GetProperty("Id");
                        var id = idProperty?.GetValue(data);
                        return CreatedAtAction("GetById", new { id }, data);
                    }

                    return new ObjectResult(data)
                    {
                        StatusCode = result.StatusCode
                    };
                }
                return new StatusCodeResult(result.StatusCode);
            }

            var errorResult = result as ErrorResult;
            return new ObjectResult(new { Errors = errorResult?.Errors })
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