using EmployeeManagement.Application.DataTransferObjects.Employee;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    /// <summary>
    /// Retrieves an employee by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="EmployeeResponseDto"/> if found; otherwise, null.</returns>
    Task<EmployeeResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all employees.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="EmployeeResponseDto"/> objects.</returns>
    Task<IEnumerable<EmployeeResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves employees belonging to a specific department.
    /// </summary>
    /// <param name="departmentId">The ID of the department.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="EmployeeResponseDto"/> objects.</returns>
    Task<IEnumerable<EmployeeResponseDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="dto">The employee data to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the newly created employee.</returns>
    Task<int> CreateAsync(EmployeeRequestDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    /// <param name="id">The ID of the employee to update.</param>
    /// <param name="dto">The updated employee data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the update succeeded; otherwise, false.</returns>
    Task<bool> UpdateAsync(int id, EmployeeRequestDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an employee.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the deletion succeeded; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    // Dashboard

    /// <summary>
    /// Gets the total number of employees in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The total number of employees.</returns>
    Task<int> GetTotalEmployeesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of employees in a specific department.
    /// </summary>
    /// <param name="departmentId">The ID of the department.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of employees in the department.</returns>
    Task<int> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of employees who joined in the last 'x' days.
    /// </summary>
    /// <param name="days">The number of days to look back.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of employees who joined within the specified period.</returns>
    Task<int> GetEmployeesJoinedInLastDaysAsync(int days, CancellationToken cancellationToken = default);
}