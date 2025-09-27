using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Entities;

public class Department : BaseEntity<int>
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
}