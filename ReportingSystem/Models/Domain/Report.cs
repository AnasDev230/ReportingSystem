using Microsoft.AspNetCore.Identity;

namespace ReportingSystem.Models.Domain
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public string UserId {  get; set; }
        public IdentityUser User { get; set; }
        public Guid ReportTypeId {  get; set; }
        public ReportType ReportType { get; set; }
        public Guid GovernorateId { get; set; }
        public Governorate Governorate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public  decimal Latitude { get; set; }    
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string Status {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ReportUpdate> ReportUpdates { get; set; }
        public ICollection<Image> Images { get; set; }=new List<Image>();
    }
}
