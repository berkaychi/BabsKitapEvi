using BabsKitapEvi.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace BabsKitapEvi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(TS.Result.Result<T> result, bool isCreation = false)
        {
            if (result.ErrorMessages is null)
            {
                if (isCreation)
                {
                    return new ObjectResult(result.Data)
                    {
                        StatusCode = 201
                    };
                }
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
                        var idProperty = data.GetType().GetProperty("Id");
                        var id = idProperty != null ? idProperty.GetValue(data) : null;
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
    }
}