using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Employee
{
    public class UpdateEmployeeRequestDto
    {
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
