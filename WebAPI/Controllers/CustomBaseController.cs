using Microsoft.AspNetCore.Mvc;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(TS.Result.Result<T> result)
        {
            if (result.ErrorMessages is null)
            {
                if (result.Data is null || typeof(T) == typeof(string))
                {
                    return new NoContentResult();
                }
                return new ObjectResult(result.Data)
                {
                    StatusCode = 200
                };
            }

            return new ObjectResult(new { Errors = result.ErrorMessages })
            {
                StatusCode = result.StatusCode
            };
        }
    }
}