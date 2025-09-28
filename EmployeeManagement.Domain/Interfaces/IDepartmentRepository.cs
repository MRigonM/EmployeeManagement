using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IDepartmentRepository : IGenericRepository<Department, int>
{
    Task<bool> HasEmployeesAsync(int departmentId, CancellationToken cancellationToken = default);
}