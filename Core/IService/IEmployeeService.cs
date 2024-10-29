using Core.DTOs;
using Infrastructure.Entities;

namespace Core.IService
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeById(int id);
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> CreateEmployee(EmployeeDto employeeDto);
        Task<Employee> UpdateEmployee(int id, EmployeeDto employeeDTO);
        Task<bool> DeleteEmployee(int id);

    }
}
