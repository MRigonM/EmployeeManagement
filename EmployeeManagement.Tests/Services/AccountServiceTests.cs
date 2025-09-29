using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Application.DataTransferObjects.Identity;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagement.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManager;
    private readonly Mock<SignInManager<AppUser>> _signInManager;
    private readonly Mock<RoleManager<IdentityRole>> _roleManager;
    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<ILogger<AccountService>> _logger;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IEmployeeService> _employeeService;

    private AccountService CreateService() =>
        new AccountService(
            _userManager.Object,
            _signInManager.Object,
            _roleManager.Object,
            _configuration.Object,
            _logger.Object,
            _mapper.Object,
            _employeeService.Object);

    public AccountServiceTests()
    {
        var store = new Mock<IUserStore<AppUser>>();
        _userManager = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);

        _signInManager = new Mock<SignInManager<AppUser>>(
            _userManager.Object,
            Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null, null, null, null
        );

        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        _roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

        _configuration = new Mock<IConfiguration>();
        _logger = new Mock<ILogger<AccountService>>();
        _mapper = new Mock<IMapper>();
        _employeeService = new Mock<IEmployeeService>();

        _configuration.Setup(c => c["Jwt:Key"]).Returns("supersupersupersecretkey123456789");
        _configuration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
    }

    [Fact]
    public async Task RegisterEmployeeAsync_ShouldSucceed_WhenValidData()
    {
        var dto = new RegisterEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            PhoneNumber = "123456789",
            DepartmentId = 1
        };

        _userManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _roleManager.Setup(r => r.RoleExistsAsync("Employee")).ReturnsAsync(true);
        _userManager.Setup(m => m.AddToRoleAsync(It.IsAny<AppUser>(), "Employee"))
            .ReturnsAsync(IdentityResult.Success);

        _employeeService.Setup(s => s.CreateAsync(It.IsAny<EmployeeCreateDto>(), default))
            .ReturnsAsync(Result<EmployeeResponseDto>.Success(new EmployeeResponseDto { Id = 1 }));

        var service = CreateService();
        var result = await service.RegisterEmployeeAsync(dto);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Employee registered successfully.");
    }

    [Fact]
    public async Task RegisterEmployeeAsync_ShouldFail_WhenIdentityFails()
    {
        var dto = new RegisterEmployeeDto
        {
            Email = "fail@example.com",
            Password = "badpass"
        };

        _userManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid password" }));

        var service = CreateService();
        var result = await service.RegisterEmployeeAsync(dto);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Description.Contains("Invalid password"));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCredentials()
    {
        var dto = new LoginDto { Email = "login@example.com", Password = "Password123!" };
        var user = new AppUser { Id = "321", Email = dto.Email };

        _userManager.Setup(m => m.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
        _signInManager.Setup(s => s.CheckPasswordSignInAsync(user, dto.Password, false))
            .ReturnsAsync(SignInResult.Success);
        _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        _configuration.Setup(c => c["Jwt:Key"]).Returns("this_is_a_super_secure_test_key_123456");
        _configuration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        var service = CreateService();

        var result = await service.LoginAsync(dto);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Token.Should().NotBeNullOrEmpty();
        result.Value.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenInvalidPassword()
    {
        var dto = new LoginDto { Email = "wrong@example.com", Password = "badpass" };
        var user = new AppUser { Id = "444", Email = dto.Email };

        _userManager.Setup(m => m.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
        _signInManager.Setup(s => s.CheckPasswordSignInAsync(user, dto.Password, false))
            .ReturnsAsync(SignInResult.Failed);

        var service = CreateService();
        var result = await service.LoginAsync(dto);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Description == "Invalid email or password.");
    }
}