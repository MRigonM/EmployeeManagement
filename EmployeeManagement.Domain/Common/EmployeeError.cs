namespace EmployeeManagement.Domain.Common;

/// <summary>
/// Defines possible errors related to employees.
/// </summary>
public static class EmployeeError
{
    public static Error NotFound(int id) => new Error("Employee.NotFound", $"Employee with ID {id} was not found.");

    public static Error NoChangesDetected => new Error("Employee.NoChanges", "No changes were detected during the operation.");

    public static Error CreationFailed => new Error("Employee.CreationFailed", "Employee creation failed. No changes were made to the database.");

    public static Error CreationUnexpectedError => new Error("Employee.CreationUnexpectedError", "An unexpected error occurred during employee creation.");

    public static Error RetrievalError => new Error("Employee.RetrievalError", "An error occurred while retrieving the list of employees.");

    public static Error UpdateUnexpectedError => new Error("Employee.UpdateUnexpectedError", "An unexpected error occurred during the update operation.");

    public static Error DeletionUnexpectedError => new Error("Employee.DeletionUnexpectedError", "An unexpected error occurred during the deletion operation.");
}