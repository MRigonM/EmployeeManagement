namespace EmployeeManagement.Application.DataTransferObjects.Employee;

public class EmployeeUpdateDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DepartmentId { get; set; }
}
