using EmployeeManagement.Application.DataTransferObjects.Department;
using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Application.Interfaces;

public interface IDepartmentService
{
    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing a <see cref="DepartmentResponseDto"/> if found,
    /// or an error if not found or retrieval fails.
    /// </returns>
    Task<Result<DepartmentResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing a collection of <see cref="DepartmentResponseDto"/> objects,
    /// or an error if retrieval fails.
    /// </returns>
    Task<Result<IEnumerable<DepartmentResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="dto">The department data to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the ID of the newly created department,
    /// or an error if creation fails.
    /// </returns>
    Task<Result<DepartmentResponseDto>> CreateAsync(DepartmentCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing department.
    /// </summary>
    /// <param name="id">The ID of the department to update.</param>
    /// <param name="dto">The updated department data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing true if the update succeeded,
    /// or an error if the department was not found, no changes were detected, or the update failed.
    /// </returns>
    Task<Result<DepartmentResponseDto>> UpdateAsync(int id, DepartmentUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a department.
    /// </summary>
    /// <param name="id">The ID of the department to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing true if the deletion succeeded,
    /// or an error if the department was not found, has employees assigned, or deletion failed.
    /// </returns>
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of departments in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the total number of departments,
    /// or an error if counting fails.
    /// </returns>
    Task<Result<DepartmentSummaryDto>> GetTotalDepartmentsAsync(CancellationToken cancellationToken = default);
}