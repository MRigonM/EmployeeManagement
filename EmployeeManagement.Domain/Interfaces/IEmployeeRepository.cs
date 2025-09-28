using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee, int>
{
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
    Task<int> CountByDepartmentAsync(int departmentId);
    Task<int> CountJoinedInLastDaysAsync(int days);
}