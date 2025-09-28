using EmployeeManagement.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult FromResult<T>(Result<T> result)
        {
            if (result.IsSuccess && result.Value is not null)
                return Ok(result.Value);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(result.Errors);
        }
    }
}