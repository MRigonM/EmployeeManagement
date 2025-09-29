using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Tests.Repositories;

public class DepartmentRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task HasEmployeesAsync_ShouldReturnTrue_WhenEmployeesExist()
    {
        var context = GetDbContext();
        var repository = new DepartmentRepository(context);

        var department = new Department { Id = 1, Name = "IT", Description = "Tech dept" };
        context.Departments.Add(department);

        context.Employees.Add(new Employee
        {
            Id = 1,
            Name = "Alice",
            Surname = "Smith",
            Email = "alice@test.com",
            PhoneNumber = "123456",
            DepartmentId = 1,
            DateOfJoining = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var result = await repository.HasEmployeesAsync(1, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task HasEmployeesAsync_ShouldReturnFalse_WhenNoEmployeesExist()
    {
        var context = GetDbContext();
        var repository = new DepartmentRepository(context);

        var department = new Department { Id = 1, Name = "HR", Description = "HR dept" };
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var result = await repository.HasEmployeesAsync(1, CancellationToken.None);

        Assert.False(result);
    }
}