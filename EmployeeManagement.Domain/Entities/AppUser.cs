using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Domain.Entities;

public class AppUser : IdentityUser, ISoftDeletion
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}