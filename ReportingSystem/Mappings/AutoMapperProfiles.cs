using AutoMapper;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Governorate;

namespace ReportingSystem.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Governorate, GovernorateDto>().ReverseMap();
        }
    }
}
