using AutoMapper;
using Core.DTOs;
using Core.IService;
using Infrastructure.Entities;
using Infrastructure.IRepository;

namespace Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }


        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAll();
                return employees.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
            return employee;
        }

        public async Task<Employee> CreateEmployee(EmployeeDto EmployeeDto)
        {
            var employee = _mapper.Map<EmployeeDto, Employee>(EmployeeDto);
            await _unitOfWork.EmployeeRepository.Insert(employee);
            await _unitOfWork.Save();
            return employee;
        }

        public async Task<Employee> UpdateEmployee(int id, EmployeeDto EmployeeDto)
        {
            var employee = await GetEmployeeById(id);
            if (employee == null)
            {
                return new Employee();
            }

            _mapper.Map(EmployeeDto, employee);
            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.Save();

            return employee;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
            if (employee == null)
            {
                return false;
            }

            await _unitOfWork.EmployeeRepository.Delete(id);
            await _unitOfWork.Save();

            return true;
        }
    }
}
