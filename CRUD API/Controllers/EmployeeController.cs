using CRUD_API.Services.Interfaces;
using CRUD_API.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;
        public EmployeeController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize]
        [HttpGet("GetAllEmployees")]
        public Task<IEnumerable<EmplyeeModel>> Get()
        {
            return _employeeService.GetAllEmployees();

        }

        [Authorize]
        [HttpGet("getEmployee/{employeeId}")]
        public Task<EmplyeeModel> GetEmployee([FromRoute] int employeeId)
        {
            return _employeeService.GetEmployee(employeeId);
        }

        [Authorize]
        [HttpPost("addNewEmployee")]
        public Task<bool> AddNewEmployee([FromBody] EmplyeeModel emplyeeModel)
        {
            return _employeeService.AddNewEmployee(emplyeeModel);
        }

        [Authorize]
        [HttpPatch("updateEmployee/{employeeId}")]
        public Task<EmplyeeModel> UpdateEmployee([FromRoute] int employeeId, [FromBody] EmplyeeModel emplyeeModel)
        {
            return _employeeService.UpdateEmployee(employeeId, emplyeeModel);
        }

        [Authorize]
        [HttpDelete("deleteEmployee/{employeeId}")]
        public Task<EmplyeeModel> DeleteEmployee([FromRoute] int employeeId)
        {
            return _employeeService.DeleteEmployee(employeeId);
        }
    }
}
