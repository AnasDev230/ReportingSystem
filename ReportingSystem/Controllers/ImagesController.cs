using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.DTO.Image;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost("{reportId}")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromRoute] Guid reportId)
        {
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
        public async Task<IActionResult> DeleteImage([FromRoute] Guid imageId)
        {
            var image = await imageRepository.DeleteAsync(imageId);
            if(image)
                return Ok("Image Deleted Successfully");
            
            return BadRequest(ModelState);
        }




        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetImageByID([FromRoute] Guid imageId)
        {
            var image = await imageRepository.GetByIdAsync(imageId);
            if (image == null)
                return NotFound();
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
        public async Task<IActionResult> GetAll([FromRoute] Guid reportId)
        {
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
