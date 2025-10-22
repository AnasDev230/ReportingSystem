using ReportingSystem.Models.DTO.Department;

namespace ReportingSystem.Models.DTO.Governorate
{
    public class GovernorateDto
    {
        public Guid GovernorateId { get; set; }
        public string Name { get; set; }
        public List<DepartmentDto>? Departments { get; set; }
    }
}
