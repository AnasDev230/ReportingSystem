using AutoMapper;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Department;
using ReportingSystem.Models.DTO.Employee;
using ReportingSystem.Models.DTO.Governorate;
using ReportingSystem.Models.DTO.ReportType;

namespace ReportingSystem.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Governorate, GovernorateDto>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<CreateDepartmentRequestDto,Department>().ReverseMap();
            CreateMap<UpdateDepartmentRequestDto, Department>().ReverseMap();



            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CreateEmployeeRequestDto, EmployeeDto>().ReverseMap();
            CreateMap<UpdateEmployeeRequestDto, EmployeeDto>().ReverseMap();


            CreateMap<ReportType, ReportTypeDto>().ReverseMap();
            CreateMap<CreateReportTypeRequestDto, ReportType>().ReverseMap();
            CreateMap<UpdateReportTypeRequestDto, ReportType>().ReverseMap();
        }
    }
}
