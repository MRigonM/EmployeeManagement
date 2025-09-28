using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork,
        ILogger<EmployeeService> logger,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<EmployeeResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving Employee with Id: {Id}", id);

            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            
            if (employee is null)
                return Result<EmployeeResponseDto>.Failure(EmployeeError.NotFound(id));

            return Result<EmployeeResponseDto>.Success(_mapper.Map<EmployeeResponseDto>(employee));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Employee with Id: {Id}", id);
            return Result<EmployeeResponseDto>.Failure(EmployeeError.RetrievalError);
        }
    }

    public async Task<Result<IEnumerable<EmployeeResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all employees...");

            var employees = await _employeeRepository.GetAllAsync(cancellationToken);
            
            if (employees is null)
                return Result<IEnumerable<EmployeeResponseDto>>.Failure(EmployeeError.RetrievalError);

            return Result<IEnumerable<EmployeeResponseDto>>.Success(_mapper.Map<IEnumerable<EmployeeResponseDto>>(employees));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            
            return Result<IEnumerable<EmployeeResponseDto>>.Failure(EmployeeError.RetrievalError);
        }
    }

    public async Task<Result<IEnumerable<EmployeeResponseDto>>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving Employees for Department: {DepartmentId}", departmentId);

            var employees = await _employeeRepository.GetByDepartmentAsync(departmentId);
            
            return Result<IEnumerable<EmployeeResponseDto>>
                .Success(_mapper.Map<IEnumerable<EmployeeResponseDto>>(employees));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Employees by Department: {DepartmentId}", departmentId);
            
            return Result<IEnumerable<EmployeeResponseDto>>.Failure(EmployeeError.RetrievalError);
        }
    }

    public async Task<Result<int>> CreateAsync(EmployeeRequestDto employeeRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating Employee: {Name} {Surname}", employeeRequest.Name, employeeRequest.Surname);

            var employee = _mapper.Map<Employee>(employeeRequest);
            employee.DateOfJoining = DateTime.UtcNow;

            var id = await _employeeRepository.AddAsync(employee, cancellationToken);
            var saved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!saved)
                return Result<int>.Failure(EmployeeError.CreationFailed);

            _logger.LogInformation("Successfully created Employee with Id: {Id}", id);
            
            return Result<int>.Success(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Employee: {Name} {Surname}", employeeRequest.Name, employeeRequest.Surname);
            
            return Result<int>.Failure(EmployeeError.CreationUnexpectedError);
        }
    }

    public async Task<Result<bool>> UpdateAsync(int id, EmployeeRequestDto employeeRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating Employee with Id: {Id}", id);

            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (employee is null)
                return Result<bool>.Failure(EmployeeError.NotFound(id));

            _mapper.Map(employeeRequest, employee);

            _employeeRepository.Update(employee);
            var updated = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return updated ? Result<bool>.Success(true)
                : Result<bool>.Failure(EmployeeError.NoChangesDetected);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Employee with Id: {Id}", id);
            return Result<bool>.Failure(EmployeeError.UpdateUnexpectedError);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting Employee with Id: {Id}", id);

            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            
            if (employee is null)
                return Result<bool>.Failure(EmployeeError.NotFound(id));

            _employeeRepository.Delete(employee);
            
            var deleted = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return deleted
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(EmployeeError.NoChangesDetected);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Employee with Id: {Id}", id);
            
            return Result<bool>.Failure(EmployeeError.DeletionUnexpectedError);
        }
    }

    public async Task<Result<int>> GetTotalEmployeesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Counting all employees...");
            var count = await _employeeRepository.CountAsync(cancellationToken);
            
            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting employees");
            
            return Result<int>.Failure(EmployeeError.RetrievalError);
        }
    }

    public async Task<Result<int>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _employeeRepository.CountByDepartmentAsync(departmentId);
            
            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting Employees by Department: {DepartmentId}", departmentId);
            
            return Result<int>.Failure(EmployeeError.RetrievalError);
        }
    }

    public async Task<Result<int>> GetEmployeesJoinedInLastDaysAsync(int days, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _employeeRepository.CountJoinedInLastDaysAsync(days);
            
            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting Employees joined in last {Days} days", days);
            
            return Result<int>.Failure(EmployeeError.RetrievalError);
        }
    }
}
