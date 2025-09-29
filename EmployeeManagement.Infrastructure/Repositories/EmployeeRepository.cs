using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository : GenericRepository<Employee, int>, IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public IQueryable<Employee> GetByDepartment(int departmentId)
    {
        return _context.Employees
            .Include(e => e.Department)
            .Where(e => e.DepartmentId == departmentId);
    }

    public async Task<int> CountByDepartmentAsync(int departmentId)
    {
        return await _context.Employees.CountAsync(e => e.DepartmentId == departmentId);
    }

    public async Task<int> CountJoinedInLastDaysAsync(int days)
    {
        return await _context.Employees.CountAsync(e => e.DateOfJoining >= DateTime.UtcNow.AddDays(-days));
    }
}