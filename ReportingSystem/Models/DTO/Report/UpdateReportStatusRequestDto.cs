namespace ReportingSystem.Models.DTO.Report
{
    public class UpdateReportStatusRequestDto
    {
        public string NewStatus { get; set; }
        public string? Comment { get; set; }
    }
}
