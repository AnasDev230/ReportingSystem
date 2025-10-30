using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.Domain
{
    public class Department
    {
        public Guid DepartmentId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
        public Guid GovernorateId { get; set; }

        public Governorate Governorate { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<ReportType>? ReportTypes { get; set; }
    }
}
