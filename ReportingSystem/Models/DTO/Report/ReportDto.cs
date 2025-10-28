namespace ReportingSystem.Models.DTO.Report
{
    public class ReportDto
    {
        public Guid ReportId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid ReportTypeId { get; set; }
        
        public Guid GovernorateId { get; set; }
        public string? GovernorateName { get; set; }

        public string UserId { get; set; }
    }
}
