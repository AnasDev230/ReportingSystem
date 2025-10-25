namespace ReportingSystem.Models.DTO.ReportType
{
    public class UpdateReportTypeRequestDto
    {
        public string ReportTypeName { get; set; }
        public bool IsActive { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
