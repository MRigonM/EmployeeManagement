using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Tests.Repositories;

public class EmployeeRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenExists()
    {
        var context = GetDbContext();
        var repository = new EmployeeRepository(context);

        var department = new Department { Id = 1, Name = "IT", Description = "Tech dept" };
        var employee = new Employee
        {
            Id = 1,
            Name = "Rigon",
            Surname = "Mejzini",
            Email = "rigon@test.com",
            PhoneNumber = "123456",
            DepartmentId = 1,
            Department = department,
            DateOfJoining = DateTime.UtcNow
        };

        context.Departments.Add(department);
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var result = await repository.GetEmployeeByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Rigon", result!.Name);
        Assert.Equal("IT", result.Department.Name);
    }

    [Fact]
    public async Task GetByDepartment_ShouldReturnEmployeesInDepartment()
    {
        var context = GetDbContext();
        var repository = new EmployeeRepository(context);

        context.Departments.Add(new Department { Id = 1, Name = "HR", Description = "HR dept" });
        context.Departments.Add(new Department { Id = 2, Name = "Finance", Description = "Finance dept" });

        context.Employees.AddRange(
            new Employee { Id = 1, Name = "Alice", Surname = "Smith", Email = "alice@test.com", PhoneNumber = "111", DepartmentId = 1, DateOfJoining = DateTime.UtcNow },
            new Employee { Id = 2, Name = "Bob", Surname = "Jones", Email = "bob@test.com", PhoneNumber = "222", DepartmentId = 1, DateOfJoining = DateTime.UtcNow },
            new Employee { Id = 3, Name = "Charlie", Surname = "Brown", Email = "charlie@test.com", PhoneNumber = "333", DepartmentId = 2, DateOfJoining = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var result = repository.GetByDepartment(1).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, e => Assert.Equal(1, e.DepartmentId));
    }

    [Fact]
    public async Task CountByDepartmentAsync_ShouldReturnCorrectCount()
    {
        var context = GetDbContext();
        var repository = new EmployeeRepository(context);

        context.Departments.Add(new Department { Id = 1, Name = "Ops", Description = "Operations dept" });

        context.Employees.AddRange(
            new Employee { Id = 1, Name = "Alice", Surname = "Smith", Email = "alice@test.com", PhoneNumber = "111", DepartmentId = 1, DateOfJoining = DateTime.UtcNow },
            new Employee { Id = 2, Name = "Bob", Surname = "Jones", Email = "bob@test.com", PhoneNumber = "222", DepartmentId = 1, DateOfJoining = DateTime.UtcNow },
            new Employee { Id = 3, Name = "Charlie", Surname = "Brown", Email = "charlie@test.com", PhoneNumber = "333", DepartmentId = 2, DateOfJoining = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var count = await repository.CountByDepartmentAsync(1);

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task CountJoinedInLastDaysAsync_ShouldReturnRecentEmployees()
    {
        var context = GetDbContext();
        var repository = new EmployeeRepository(context);

        context.Departments.Add(new Department { Id = 1, Name = "QA", Description = "Quality Assurance" });

        context.Employees.AddRange(
            new Employee { Id = 1, Name = "Recent", Surname = "Test", Email = "recent@test.com", PhoneNumber = "444", DepartmentId = 1, DateOfJoining = DateTime.UtcNow },
            new Employee { Id = 2, Name = "Old", Surname = "Tester", Email = "old@test.com", PhoneNumber = "555", DepartmentId = 1, DateOfJoining = DateTime.UtcNow.AddDays(-10) }
        );
        await context.SaveChangesAsync();

        var count = await repository.CountJoinedInLastDaysAsync(5);

        Assert.Equal(1, count);
    }
}
