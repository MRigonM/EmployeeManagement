using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee, int>
{
    IQueryable<Employee> GetByDepartment(int departmentId);
    Task<int> CountByDepartmentAsync(int departmentId);
    Task<int> CountJoinedInLastDaysAsync(int days);
}