using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Employee
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
