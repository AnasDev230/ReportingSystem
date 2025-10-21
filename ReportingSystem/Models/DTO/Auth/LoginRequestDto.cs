using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [RegularExpression(@"^(\+?9639\d{8}|09\d{8})$", ErrorMessage = "الرجاء إدخال رقم موبايل سوري صحيح.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
