namespace ReportingSystem.Models.DTO.ReportUpdate
{
    public class ReportUpdateByReportIdDto
    {
        public Guid ReportUpdateId { get; set; }
        public Guid ReportId { get; set; }
        public string UserId {  get; set; }
        public string Status { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
    }
}
