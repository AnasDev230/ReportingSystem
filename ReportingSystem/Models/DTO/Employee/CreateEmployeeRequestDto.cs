using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Employee
{
    public class CreateEmployeeRequestDto
    {




        [Required]
        [MinLength(5)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\+?9639\d{8}|09\d{8})$", ErrorMessage = "الرجاء إدخال رقم موبايل سوري صحيح.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //public string[] Roles { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
