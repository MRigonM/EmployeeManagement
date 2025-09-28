using EmployeeManagement.Application.DataTransferObjects.Department;

namespace EmployeeManagement.Application.Interfaces;

public interface IDepartmentService
{
    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="DepartmentResponseDto"/> if found; otherwise, null.</returns>
    Task<DepartmentResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="DepartmentResponseDto"/> objects.</returns>
    Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="dto">The department data to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the newly created department.</returns>
    Task<int> CreateAsync(DepartmentRequestDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing department.
    /// </summary>
    /// <param name="id">The ID of the department to update.</param>
    /// <param name="dto">The updated department data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the update succeeded; otherwise, false.</returns>
    Task<bool> UpdateAsync(int id, DepartmentRequestDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a department.
    /// </summary>
    /// <param name="id">The ID of the department to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the deletion succeeded; otherwise, false. Returns false if employees are still assigned to the department.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of departments in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The total number of departments.</returns>
    Task<int> GetTotalDepartmentsAsync(CancellationToken cancellationToken = default);
}