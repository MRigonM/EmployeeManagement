using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Department, int>, IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> HasEmployeesAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AnyAsync(e => e.DepartmentId == departmentId, cancellationToken);
    }
}