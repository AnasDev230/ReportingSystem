namespace ReportingSystem.Models.DTO.ReportType
{
    public class CreateReportTypeRequestDto
    {
        public string ReportTypeName { get; set; }
        public bool IsActive { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
