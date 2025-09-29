namespace EmployeeManagement.Domain.Entities;

public class Employee : BaseEntity<int>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfJoining { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}