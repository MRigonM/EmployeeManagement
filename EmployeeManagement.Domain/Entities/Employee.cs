using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Employee : BaseEntity<int>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Phone]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    public DateTime DateOfJoining { get; set; }

    [ForeignKey("Department")]
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}