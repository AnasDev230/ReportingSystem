using AutoMapper;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Department;
using ReportingSystem.Models.DTO.Governorate;

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
        }
    }
}
