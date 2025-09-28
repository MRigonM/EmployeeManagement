using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Department;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DepartmentService> _logger;
    private readonly IMapper _mapper;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork,
        ILogger<DepartmentService> logger,
        IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<DepartmentResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving Department with Id: {Id}", id);

            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (department is null)
                return Result<DepartmentResponseDto>.Failure(DepartmentError.NotFound(id));

            return Result<DepartmentResponseDto>.Success(_mapper.Map<DepartmentResponseDto>(department));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Department with Id: {Id}", id);
            return Result<DepartmentResponseDto>.Failure(DepartmentError.RetrievalError);
        }
    }

    public async Task<Result<IEnumerable<DepartmentResponseDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all departments...");

            var departments = await _departmentRepository
                .GetAll()
                .ToListAsync(cancellationToken);

            if (departments == null || !departments.Any())
                return Result<IEnumerable<DepartmentResponseDto>>.Failure(DepartmentError.RetrievalError);

            return Result<IEnumerable<DepartmentResponseDto>>.Success(
                _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            return Result<IEnumerable<DepartmentResponseDto>>.Failure(DepartmentError.RetrievalError);
        }
    }

    public async Task<Result<int>> CreateAsync(DepartmentRequestDto departmentRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating Department: {Name}", departmentRequest.Name);

            var department = _mapper.Map<Department>(departmentRequest);

            var id = await _departmentRepository.AddAsync(department, cancellationToken);
            var saved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!saved)
                return Result<int>.Failure(DepartmentError.CreationFailed);

            _logger.LogInformation("Successfully created Department with Id: {Id}", id);
            return Result<int>.Success(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Department: {Name}", departmentRequest.Name);
            return Result<int>.Failure(DepartmentError.CreationUnexpectedError);
        }
    }

    public async Task<Result<bool>> UpdateAsync(int id, DepartmentRequestDto departmentRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating Department with Id: {Id}", id);

            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (department is null)
                return Result<bool>.Failure(DepartmentError.NotFound(id));

            _mapper.Map(departmentRequest, department);

            _departmentRepository.Update(department);
            var updated = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return updated
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(DepartmentError.NoChangesDetected);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Department with Id: {Id}", id);
            return Result<bool>.Failure(DepartmentError.UpdateUnexpectedError);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting Department with Id: {Id}", id);

            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (department is null)
                return Result<bool>.Failure(DepartmentError.NotFound(id));

            if (await _departmentRepository.HasEmployeesAsync(id, cancellationToken))
                return Result<bool>.Failure(DepartmentError.HasEmployees(id));

            _departmentRepository.Delete(department);
            var deleted = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return deleted
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(DepartmentError.NoChangesDetected);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Department with Id: {Id}", id);
            return Result<bool>.Failure(DepartmentError.DeletionUnexpectedError);
        }
    }

    public async Task<Result<int>> GetTotalDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Counting all departments...");
            var count = await _departmentRepository.CountAsync(cancellationToken);
            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting departments");
            return Result<int>.Failure(DepartmentError.RetrievalError);
        }
    }
}
