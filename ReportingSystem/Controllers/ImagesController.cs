using System.Security.Claims;
using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Image;
using ReportingSystem.Repositories.Implementation;
using ReportingSystem.Repositories.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IReportRepository reportRepository;
        private readonly IEmployeeRepository employeeRepository;

        public ImagesController(IImageRepository imageRepository,IReportRepository reportRepository,EmployeeRepository employeeRepository)
        {
            this.imageRepository = imageRepository;
            this.reportRepository = reportRepository;
            this.employeeRepository = employeeRepository;
        }
        [HttpPost("{reportId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromRoute] Guid reportId)
        {

            var report=await reportRepository.GetByIdAsync(reportId);
            if(report==null)
                return NotFound("Report Not Found!");





            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            
            if (userId != report.UserId)
                return Forbid("You do not have permission to access this resource.");















            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var Image = new Models.Domain.Image
                {
                    ReportId = reportId,
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    Title = file.FileName,
                    DateCreated = DateTime.Now,
                };
                Image = await imageRepository.Upload(file, Image);

                ImageDto response = new ImageDto
                {
                    ImageId=Image.ImageId,
                    ReportId=Image.ReportId,
                    DateCreated = DateTime.Now,
                    Url = Image.Url
                };

                return Ok(response);
            }
            return BadRequest(ModelState);
        }


      

        [HttpDelete("{imageId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid imageId)
        {

            var image=await imageRepository.GetByIdAsync(imageId);


            var report = await reportRepository.GetByIdAsync(image.ReportId);
            if (report == null)
                return NotFound("Report Not Found!");





            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");


            if (User.IsInRole("User"))
            {
                if (userId != report.UserId)
                    return Forbid("You do not have permission to access this resource.");
            }


            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var employee = await employeeRepository.GetByUserIDAsync(userId);
                if (employee == null)
                    return Forbid("You do not have permission to access this resource.");

                if (employee.DepartmentId != report.ReportType.DepartmentId)
                    return Forbid("You can only access reports in your own department.");
            }





            if (await imageRepository.DeleteAsync(imageId))
                return Ok("Image Deleted Successfully");
            
            return BadRequest(ModelState);
        }




        [HttpGet("{imageId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetImageByID([FromRoute] Guid imageId)
        {
            var image = await imageRepository.GetByIdAsync(imageId);
            if (image == null)
                return NotFound();





            var report = await reportRepository.GetByIdAsync(image.ReportId);
            if (report == null)
                return NotFound("Report Not Found!");





            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");


            if (User.IsInRole("User"))
            {
                if (userId != report.UserId)
                    return Forbid("You do not have permission to access this resource.");
            }
                

            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var employee = await employeeRepository.GetByUserIDAsync(userId);
                if (employee == null)
                    return Forbid("You do not have permission to access this resource.");

                if (employee.DepartmentId != report.ReportType.DepartmentId)
                    return Forbid("You can only access reports in your own department.");
            }
             


            var response = new ImageDto
            {
                ImageId = imageId,
                ReportId = image.ReportId,
                DateCreated = image.DateCreated,
                Url = image.Url
            };
            return Ok(response);
        }


        [HttpGet]
        [Authorize(Roles = "No Roles for Now")]
        public async Task<IActionResult> GetAll()
        {
            var images = await imageRepository.GetAll();
            var response = new List<ImageDto>();
            foreach (var Image in images)
            {
                response.Add(new ImageDto
                {
                   ImageId=Image.ImageId,
                   ReportId=Image.ReportId,
                    DateCreated = Image.DateCreated,
                    Url = Image.Url
                });
            }
            return Ok(response);
        }




        [HttpGet("GetImagesByReportId/{reportId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetAllImagesByReportId([FromRoute] Guid reportId)
        {

            var report = await reportRepository.GetByIdAsync(reportId);
            if (report == null)
                return NotFound("Report Not Found!");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            if (User.IsInRole("User"))
            {
                if (userId != report.UserId)
                    return Forbid("You do not have permission to access this resource.");
            }




            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var employee=await employeeRepository.GetByUserIDAsync(userId);
                if (employee == null)
                    return Forbid("You do not have permission to access this resource.");

                if (employee.DepartmentId!=report.ReportType.DepartmentId)
                    return Forbid("You can only access reports in your own department.");
            }




            var images = await imageRepository.GetByReportIdAsync(reportId);
            var response = new List<ImageDto>();
            foreach (var Image in images)
            {
                response.Add(new ImageDto
                {
                    ImageId = Image.ImageId,
                    ReportId = Image.ReportId,
                    DateCreated = Image.DateCreated,
                    Url = Image.Url
                });
            }
            return Ok(response);
        }





        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpej", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size Cannot Be More Than 10 Mb");
            }
        }
    }
}
