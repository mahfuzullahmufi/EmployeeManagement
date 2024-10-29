using AutoMapper;
using Core.DTOs;
using Core.Services;
using Infrastructure.Entities;
using Infrastructure.IRepository;
using Moq;

namespace EmployeeManagementTest
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            _employeeService = new EmployeeService(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsEmployee()
        {
            // Arrange
            int id = 1;
            var employee = new Employee { Id = id, Name = "Mufi", Designation = "Associate Software Engineer" };
            unitOfWorkMock.Setup(u => u.EmployeeRepository.GetById(id)).ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mufi", result.Name);
            Assert.Equal("Associate Software Engineer", result.Designation);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Mufi", Designation = "Engineer" },
            new Employee { Id = 2, Name = "Erfan", Designation = "Developer" }
        };
            unitOfWorkMock.Setup(u => u.EmployeeRepository.GetAll()).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployees();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Mufi", result[0].Name);
            Assert.Equal("Engineer", result[0].Designation);
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Name = "Erfan", Designation = "Software Engineer" };
            var employee = new Employee { Name = "Erfan", Designation = "Software Engineer" };

            mapperMock.Setup(m => m.Map<EmployeeDto, Employee>(employeeDto)).Returns(employee);
            unitOfWorkMock.Setup(u => u.EmployeeRepository.Insert(employee)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);

            // Act
            var createdEmployee = await _employeeService.CreateEmployee(employeeDto);

            // Assert
            Assert.NotNull(createdEmployee);
            Assert.Equal("Erfan", createdEmployee.Name);
            Assert.Equal("Software Engineer", createdEmployee.Designation);
            unitOfWorkMock.Verify(u => u.EmployeeRepository.Insert(It.IsAny<Employee>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_ShouldReturnUpdatedEmployee()
        {
            // Arrange
            int id = 1008;
            var employee = new Employee { Id = id, Name = "Imran", Designation = "Developer" };
            var employeeDto = new EmployeeDto { Name = "Imran Habibul Shah", Designation = "Software Engineer" };

            unitOfWorkMock.Setup(u => u.EmployeeRepository.GetById(id)).ReturnsAsync(employee);
            mapperMock.Setup(m => m.Map(employeeDto, employee)).Callback<EmployeeDto, Employee>((src, dest) => {
                dest.Name = src.Name;
                dest.Designation = src.Designation;
            });
            unitOfWorkMock.Setup(u => u.EmployeeRepository.Update(employee)).Verifiable();
            unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);

            // Act
            var updatedEmployee = await _employeeService.UpdateEmployee(id, employeeDto);

            // Assert
            Assert.NotNull(updatedEmployee);
            Assert.Equal("Imran Habibul Shah", updatedEmployee.Name);
            Assert.Equal("Software Engineer", updatedEmployee.Designation);
            unitOfWorkMock.Verify(u => u.EmployeeRepository.Update(It.IsAny<Employee>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnTrueIfDeleted()
        {
            // Arrange
            int id = 1008;
            var employee = new Employee { Id = id, Name = "Imran", Designation = "Software Engineer" };

            unitOfWorkMock.Setup(u => u.EmployeeRepository.GetById(id)).ReturnsAsync(employee);
            unitOfWorkMock.Setup(u => u.EmployeeRepository.Delete(id)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeService.DeleteEmployee(id);

            // Assert
            Assert.True(result);
            unitOfWorkMock.Verify(u => u.EmployeeRepository.Delete(id), Times.Once);
            unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }
    }
}
