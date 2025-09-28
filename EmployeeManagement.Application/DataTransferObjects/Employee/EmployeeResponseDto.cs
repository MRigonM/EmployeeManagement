namespace EmployeeManagement.Application.DataTransferObjects.Employee;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfJoining { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}