namespace ReportingSystem.Models.Domain
{
    public class ReportType
    {
        public Guid ReportTypeId { get; set; } = Guid.NewGuid();
        public string ReportTypeName { get; set; } = string.Empty;
        public bool IsActive {  get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}
