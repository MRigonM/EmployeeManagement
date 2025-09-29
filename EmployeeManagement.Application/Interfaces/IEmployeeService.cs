using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    /// <summary>
    /// Retrieves an employee by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing an <see cref="EmployeeResponseDto"/> if found,
    /// or an error if not found or retrieval fails.
    /// </returns>
    Task<Result<EmployeeResponseDto>> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all employees in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing a collection of <see cref="EmployeeResponseDto"/> objects,
    /// or an error if retrieval fails.
    /// </returns>
    Task<Result<IEnumerable<EmployeeResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all employees assigned to a specific department.
    /// </summary>
    /// <param name="departmentId">The ID of the department.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing a collection of <see cref="EmployeeResponseDto"/> objects,
    /// or an error if retrieval fails.
    /// </returns>
    Task<Result<IEnumerable<EmployeeResponseDto>>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="dto">The employee data to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the ID of the newly created employee,
    /// or an error if creation fails.
    /// </returns>
    Task<Result<EmployeeResponseDto>> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    /// <param name="id">The ID of the employee to update.</param>
    /// <param name="dto">The updated employee data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing true if the update succeeded,
    /// or an error if the employee was not found, no changes were detected, or the update failed.
    /// </returns>
    Task<Result<EmployeeResponseDto>> UpdateAsync(int id,EmployeeUpdateDto employeeUpdate,CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an employee.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing true if the deletion succeeded,
    /// or an error if the employee was not found or deletion failed.
    /// </returns>
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the total number of employees in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the total number of employees,
    /// or an error if counting fails.
    /// </returns>
    Task<Result<int>> GetTotalEmployeesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of employees within a specific department.
    /// </summary>
    /// <param name="departmentId">The ID of the department.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the number of employees in the department,
    /// or an error if counting fails.
    /// </returns>
    Task<Result<int>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of employees who joined in the last 'x' days.
    /// </summary>
    /// <param name="days">The number of days to look back.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the number of employees who joined,
    /// or an error if counting fails.
    /// </returns>
    Task<Result<int>> GetEmployeesJoinedInLastDaysAsync(int days, CancellationToken cancellationToken = default);
}