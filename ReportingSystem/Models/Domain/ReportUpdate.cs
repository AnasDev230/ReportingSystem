namespace ReportingSystem.Models.Domain
{
    public class ReportUpdate
    {
        public Guid ReportUpdateId { get; set; }
        public Guid ReportId { get; set; }
        public Report Report { get; set; }


        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;    
        public string Status { get; set; } 
        public string? Comment { get; set; }
    }
}
