using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagement.Tests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ILogger<EmployeeService>> _logger = new();

    private EmployeeService CreateService() =>
        new EmployeeService(_employeeRepo.Object, _unitOfWork.Object, _logger.Object, _mapper.Object);

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        var employee = new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john@doe.com", PhoneNumber = "123" };
        var dto = new EmployeeResponseDto { Id = 1, Name = "John", Surname = "Doe" };

        _employeeRepo.Setup(r => r.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
        _mapper.Setup(m => m.Map<EmployeeResponseDto>(employee)).Returns(dto);

        var service = CreateService();
        var result = await service.GetByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedEmployee_WhenSuccessful()
    {
        var dto = new EmployeeCreateDto { Name = "Jane", Surname = "Doe", Email = "jane@doe.com", PhoneNumber = "555", DepartmentId = 1 };
        var employee = new Employee { Id = 5, Name = "Jane", Surname = "Doe", Email = "jane@doe.com", PhoneNumber = "555", DepartmentId = 1 };
        var response = new EmployeeResponseDto { Id = 5, Name = "Jane", Surname = "Doe" };

        _mapper.Setup(m => m.Map<Employee>(dto)).Returns(employee);
        _employeeRepo.Setup(r => r.AddAsync(employee, default)).ReturnsAsync(employee.Id);
        _unitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _employeeRepo.Setup(r => r.GetEmployeeByIdAsync(employee.Id)).ReturnsAsync(employee);
        _mapper.Setup(m => m.Map<EmployeeResponseDto>(employee)).Returns(response);

        var service = CreateService();
        var result = await service.CreateAsync(dto);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(5);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedEmployee_WhenSuccessful()
    {
        var employee = new Employee { Id = 1, Name = "Old", Surname = "Name", Email = "old@doe.com", PhoneNumber = "111" };
        var updateDto = new EmployeeUpdateDto { Name = "New", Surname = "Name", Email = "new@doe.com", PhoneNumber = "222" };
        var updatedEmployee = new Employee { Id = 1, Name = "New", Surname = "Name", Email = "new@doe.com", PhoneNumber = "222" };
        var response = new EmployeeResponseDto { Id = 1, Name = "New", Surname = "Name" };

        _employeeRepo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(employee);
        _mapper.Setup(m => m.Map(updateDto, employee)).Callback(() => {
            employee.Name = updateDto.Name;
            employee.Email = updateDto.Email;
            employee.PhoneNumber = updateDto.PhoneNumber;
        });
        _unitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _employeeRepo.Setup(r => r.GetEmployeeByIdAsync(1)).ReturnsAsync(updatedEmployee);
        _mapper.Setup(m => m.Map<EmployeeResponseDto>(updatedEmployee)).Returns(response);

        var service = CreateService();
        var result = await service.UpdateAsync(1, updateDto);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("New");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenEmployeeDoesNotExist()
    {
        _employeeRepo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Employee?)null);
        var service = CreateService();

        var result = await service.DeleteAsync(99);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == "Employee.NotFound");
    }
}