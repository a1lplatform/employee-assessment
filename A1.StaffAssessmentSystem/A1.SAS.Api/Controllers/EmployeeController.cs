using A1.SAS.Api.Dtos;
using A1.SAS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace A1.SAS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all Employee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployee()
        {

            var employees = await _employeeService.GetEmployeesAsync();

            return Ok(employees);
        }

        /// <summary>
        /// Search Employee By Id Async
        /// </summary>
        /// <param name="keyString"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeeByIdAsync(Guid id)
        {

            var employeeDto = await _employeeService.GetEmployeeByIdAsync(id);

            return Ok(employeeDto);
        }

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Guid>> Create([FromForm] PostEmployeeDto employeeDto)
        {
            var files = Request.Form.Files;

            var employee = await _employeeService.AddEmployeeAsync(employeeDto, files);

            return Ok(employee);
        }

        /// <summary>
        /// Update Employee
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update([FromForm] PostEmployeeDto employeeDto)
        {
            var files = Request.Form.Files;

            await _employeeService.UpdateEmployeeAsync(employeeDto, files);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            await _employeeService.DeleteEmployeeAsync(id);

            return NoContent();
        }
    }
}
