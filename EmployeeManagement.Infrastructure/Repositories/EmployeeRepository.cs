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

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Employees.Where(e => e.DepartmentId == departmentId).ToListAsync();
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