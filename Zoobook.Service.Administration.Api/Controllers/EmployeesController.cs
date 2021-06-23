using System.Threading.Tasks;
using Zoobook.Models;
using Zoobook.Service.Administration.Domain;
using Zoobook.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Zoobook.Service.Administration.Api
{
    /// <summary>
    /// Zoobook Systems LLC Employees Controller
    /// </summary>
    [ApiController]
    [Route(Constants.ApiRouteEmployees)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesSvc;

        public EmployeesController(
            IEmployeesService employeesSvc)
        {
            _employeesSvc = employeesSvc;
        }

        /// <summary>
        /// Searches the employee data using the provided search criteria
        /// </summary>
        /// <param name="request">Employee Search Request (Search Criteria)</param>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult> SearchAsync([FromBody] EmployeeRequestDto request)
        {
            var response = await _employeesSvc.SearchAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Creates the employee based from the provided Employee details
        /// </summary>
        /// <param name="payload">Employee Details</param>
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] EmployeeCreateDto payload)
        {
            var response = await _employeesSvc.CreateAsync(payload);
            return Ok(response);
        }

        /// <summary>
        /// Updates the details of the employee with provided Id
        /// </summary>
        /// <param name="entityId">Employee Id to Update</param>
        /// <param name="payload">Employee Details</param>
        [HttpPut]
        [Route("{entityId}")]
        public async Task<ActionResult> UpdateAsync(long entityId, [FromBody] EmployeeUpdateDto payload)
        {
            var response = await _employeesSvc.UpdateAsync(entityId, payload);
            return Ok(response);
        }

        /// <summary>
        /// Deletes the employee with provided id from the database
        /// </summary>
        /// <param name="entityId">Employee Id to Delete</param>
        [HttpDelete]
        [Route("{entityId}")]
        public async Task<ActionResult> DeleteAsync(long entityId)
        {
            var response = await _employeesSvc.DeleteAsync(entityId);
            return Ok(response);
        }
    }
}
