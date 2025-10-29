namespace ReportingSystem.Models.DTO.ReportUpdate
{
    public class ReportUpdateDto
    {
        public Guid ReportUpdateId { get; set; }
        public Guid ReportId { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
