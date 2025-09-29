using AutoMapper;
using EmployeeManagement.Application.DataTransferObjects.Department;
using EmployeeManagement.Application.DataTransferObjects.Employee;
using EmployeeManagement.Application.DataTransferObjects.Identity;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        CreateMap<EmployeeCreateDto, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfJoining, opt => opt.Ignore());
        
        CreateMap<EmployeeUpdateDto, Employee>()
            .ForAllMembers(opt => opt.Condition(
                (src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string str && string.IsNullOrWhiteSpace(str))
            ));

        CreateMap<RegisterEmployeeDto, EmployeeCreateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

        CreateMap<Department, DepartmentResponseDto>();
        CreateMap<DepartmentCreateDto, Department>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<DepartmentUpdateDto, Department>()
            .ForAllMembers(opt => opt.Condition(
                (src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string str && string.IsNullOrWhiteSpace(str))
            ));
    }
}