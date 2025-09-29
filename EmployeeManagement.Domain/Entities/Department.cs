namespace EmployeeManagement.Domain.Entities;

public class Department : BaseEntity<int>
{
    public string Name { get; set; }

    public string Description { get; set; }
}