using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Department;
using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        CreateMap<EmployeeRequestDto, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfJoining, opt => opt.Ignore()); 

        CreateMap<Department, DepartmentResponseDto>();
        CreateMap<DepartmentRequestDto, Department>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}