using Core.DTOs;
using Core.IService;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployees();
            return Ok(employees);
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            return Ok(employee);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateEmployee)}");
                return BadRequest(ModelState);
            }

            var result = await _employeeService.CreateEmployee(employeeDto);

            return Ok(result);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employeeDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateEmployee)}");
                return BadRequest(ModelState);
            }

            var employee = await _employeeService.UpdateEmployee(id, employeeDTO);
            if (employee.Id == 0)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateEmployee)}");
                return BadRequest("Submitted data is invalid");
            }

            return Ok(employee);

        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteEmployee)}");
                return BadRequest();
            }

            var result = await _employeeService.DeleteEmployee(id);
            if (!result)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteEmployee)}");
                return BadRequest("Submitted data is invalid");
            }

            return NoContent();

        }
    }
}
