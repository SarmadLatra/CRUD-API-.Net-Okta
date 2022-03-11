using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet("GetCustomerInformation")]
        public IActionResult Index()
        {
            return Ok(new { CustomerName = "Sam", Email = "sam14@gmail.com", AccountBalance = "150000" });
        }
    }
}
