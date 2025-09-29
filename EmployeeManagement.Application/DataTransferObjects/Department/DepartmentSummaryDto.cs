namespace EmployeeManagement.Application.DataTransferObjects.Department;

public class DepartmentSummaryDto
{
    public int TotalDepartments { get; set; }
    public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
}
