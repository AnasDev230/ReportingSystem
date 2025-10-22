using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Department
{
    public class UpdateDepartmentRequestDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public Guid GovernorateId { get; set; }
    }
}
