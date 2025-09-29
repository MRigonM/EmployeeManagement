using EmployeeManagement.Application.DataTransferObjects.Employee;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Employee;

public class EmpoyeeUpdateValidator : AbstractValidator<EmployeeCreateDto>
{
    public EmpoyeeUpdateValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.Surname)
            .MaximumLength(50).WithMessage("Surname cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?\d{7,15}$").WithMessage("Phone number must be 7–15 digits and may start with +.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("DepartmentId must be greater than 0.");
    }
}