using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Identity;
using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Application.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;
    private readonly IEmployeeService _employeeService;

    public AccountService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<AccountService> logger,
        IMapper mapper,
        IEmployeeService employeeService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
        _employeeService = employeeService;
    }

    public async Task<Result<string>> RegisterEmployeeAsync(RegisterEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new Employee with email: {Email}", dto.Email);

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure($"Employee registration failed: {errors}");
            }

            if (!await _roleManager.RoleExistsAsync("Employee"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            await _userManager.AddToRoleAsync(user, "Employee");

            var employeeDto = new EmployeeCreateDto
            {
                Name = dto.FirstName,
                Surname = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId
            };

            var employeeResult = await _employeeService.CreateAsync(employeeDto, cancellationToken);
            if (!employeeResult.IsSuccess)
            {
                _logger.LogWarning("Identity created but Employee entity failed for {Email}", dto.Email);
                return Result<string>.Failure("Employee entity creation failed.");
            }

            _logger.LogInformation("Successfully registered Employee with UserId: {UserId}", user.Id);
            return Result<string>.Success("Employee registered successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while registering Employee: {Email}", dto.Email);
            return Result<string>.Failure("Unexpected error during employee registration.");
        }
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Result<AuthResponseDto>.Failure("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Result<AuthResponseDto>.Failure("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Employee";

            int? departmentId = null;
            if (role == "Employee")
            {
                var employee = (await _employeeService.GetAllAsync(cancellationToken))
                    .Value?.FirstOrDefault(e => e.Email == user.Email);
                departmentId = employee?.DepartmentId;
            }

            var token = GenerateJwtToken(user, role);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Role = role,
                DepartmentId = departmentId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while logging in: {Email}", dto.Email);
            return Result<AuthResponseDto>.Failure("Unexpected error during login.");
        }
    }

    private string GenerateJwtToken(AppUser user, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
