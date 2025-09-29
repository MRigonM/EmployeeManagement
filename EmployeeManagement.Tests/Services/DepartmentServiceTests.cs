using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Department;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;


namespace EmployeeManagement.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ILogger<DepartmentService>> _logger = new();

    private DepartmentService CreateService() =>
        new DepartmentService(_departmentRepo.Object, _unitOfWork.Object, _logger.Object, _mapper.Object);

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDepartment_WhenExists()
    {
        var dept = new Department { Id = 1, Name = "HR", Description = "Human Resources" };
        var dto = new DepartmentResponseDto { Id = 1, Name = "HR" };

        _departmentRepo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(dept);
        _mapper.Setup(m => m.Map<DepartmentResponseDto>(dept)).Returns(dto);

        var service = CreateService();
        var result = await service.GetByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnDepartment_WhenSuccessful()
    {
        var createDto = new DepartmentCreateDto { Name = "Finance", Description = "Handles money" };
        var dept = new Department { Id = 2, Name = "Finance", Description = "Handles money" };
        var response = new DepartmentResponseDto { Id = 2, Name = "Finance" };

        _mapper.Setup(m => m.Map<Department>(createDto)).Returns(dept);
        _departmentRepo.Setup(r => r.AddAsync(dept, default)).ReturnsAsync(dept.Id);
        _unitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _departmentRepo.Setup(r => r.GetByIdAsync(dept.Id, default)).ReturnsAsync(dept);
        _mapper.Setup(m => m.Map<DepartmentResponseDto>(dept)).Returns(response);

        var service = CreateService();
        var result = await service.CreateAsync(createDto);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(2);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenDepartmentHasEmployees()
    {
        var dept = new Department { Id = 3, Name = "Sales", Description = "Handles sales" };
        _departmentRepo.Setup(r => r.GetByIdAsync(3, default)).ReturnsAsync(dept);
        _departmentRepo.Setup(r => r.HasEmployeesAsync(3, default)).ReturnsAsync(true);

        var service = CreateService();
        var result = await service.DeleteAsync(3);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetTotalDepartmentsAsync_ShouldReturnCount()
    {
        _departmentRepo.Setup(r => r.CountAsync(default)).ReturnsAsync(5);

        var service = CreateService();
        var result = await service.GetTotalDepartmentsAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value!.TotalDepartments.Should().Be(5);
    }
}