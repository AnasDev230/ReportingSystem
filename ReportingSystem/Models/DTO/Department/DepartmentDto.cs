namespace ReportingSystem.Models.DTO.Department
{
    public class DepartmentDto
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid GovernorateId { get; set; }
    }
}
