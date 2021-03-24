using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsApi.Controllers
{
    public class DemoController : ControllerBase
    {
        private readonly IProviderServerStatus _statusService;

        public DemoController(IProviderServerStatus statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("/status")]
        public ActionResult<GetStatusResponse> GetTheStatus()
        {
            GetStatusResponse response = _statusService.GetMyStatus();
            return Ok(response);
        }

        [HttpGet("/products/{productId:int}")]
        public ActionResult LookupProduct(int productId)
        {
            return Ok("This is your product! " + productId);
        }

        [HttpGet("/employees")]
        public ActionResult GetEmployees([FromQuery] string department = "All")
        {
            return Ok("Getting Employees Collection (" + department + ")");
        }

        [HttpGet("/whoami")]
        public ActionResult WhoAmI([FromHeader(Name ="User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }

        [HttpPost("/employees")]
        public ActionResult HireAnEmployee([FromBody] PostEmployeeRequest employeeToHire)
        {
            return Ok($"Hiring {employeeToHire.lastName} in the {employeeToHire.department} department.");
        }
    }

    public class GetStatusResponse
    {
        public string message { get; set; }
        public DateTime lastChecked { get; set; }
    }

    public class PostEmployeeRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string department { get; set; }
    }
}
