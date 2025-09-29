using EmployeeManagement.Application.DataTransferObjects.Department;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Department
{
    public class DepartmentCreateValidator : AbstractValidator<DepartmentCreateDto>
    {
        public DepartmentCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");
        }
    }
}