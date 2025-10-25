namespace ReportingSystem.Models.DTO.ReportType
{
    public class ReportTypeDto
    {
        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
