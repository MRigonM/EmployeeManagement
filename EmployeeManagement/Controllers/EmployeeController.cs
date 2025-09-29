using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

public class EmployeeController : ApiBaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetAllAsync(cancellationToken);
        return FromResult(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _employeeService.GetByIdAsync(id);
        return FromResult(result);
    }

    [HttpGet("ByDepartment/{departmentId:int}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetByDepartment(int departmentId, CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetByDepartmentAsync(departmentId, cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await _employeeService.CreateAsync(dto, cancellationToken);
        return FromResult(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await _employeeService.UpdateAsync(id, dto, cancellationToken);
        return FromResult(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _employeeService.DeleteAsync(id, cancellationToken);
        return FromResult(result);
    }

    [HttpGet("Count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTotalEmployees(CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetTotalEmployeesAsync(cancellationToken);
        return FromResult(result);
    }

    [HttpGet("Count/Department/{departmentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEmployeesByDepartment(int departmentId, CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetEmployeesByDepartmentAsync(departmentId, cancellationToken);
        return FromResult(result);
    }

    [HttpGet("Count/JoinedLastDays/{days:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEmployeesJoinedInLastDays(int days, CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetEmployeesJoinedInLastDaysAsync(days, cancellationToken);
        return FromResult(result);
    }
}
