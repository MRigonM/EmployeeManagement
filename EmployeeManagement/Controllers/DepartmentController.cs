using EmployeeManagement.Application.DataTransferObjects.Department;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

public class DepartmentController : ApiBaseController
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetAllAsync(cancellationToken);
        return FromResult(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetByIdAsync(id, cancellationToken);
        return FromResult(result);
    }

    [HttpPost("Create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] DepartmentRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _departmentService.CreateAsync(dto, cancellationToken);
        return FromResult(result);
    }
    
    [HttpPut("Update/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _departmentService.UpdateAsync(id, dto, cancellationToken);
        return FromResult(result);
    }
    
    [HttpDelete("Delete/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _departmentService.DeleteAsync(id, cancellationToken);
        return FromResult(result);
    }
    
    [HttpGet("Count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTotalDepartments(CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetTotalDepartmentsAsync(cancellationToken);
        return FromResult(result);
    }
}