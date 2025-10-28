namespace ReportingSystem.Models.DTO.Report
{
    public class UpdateReportRequestDto
    {
        public Guid ReportTypeId { get; set; }
        public Guid GovernorateId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; } = "Pending";
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
