using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.Domain
{
    public class Governorate
    {
        public Guid GovernorateId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<Department>? Departments { get; set; }
    }
}
