using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.DTO.Auth
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
