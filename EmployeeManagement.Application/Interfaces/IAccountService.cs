using EmployeeManagement.Application.DataTransferObjects.Identity;
using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Application.Interfaces;

public interface IAccountService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<Result<string>> RegisterEmployeeAsync(RegisterEmployeeDto dto, CancellationToken cancellationToken = default);
}