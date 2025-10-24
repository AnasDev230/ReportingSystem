using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Employee
{
    public class LoginEmployeeRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
