using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly SystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ImageRepository(SystemDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteAsync(Guid imageId)
        {
            var image = await dbContext.Images.FindAsync(imageId);
            if (image == null)
            {
                return false;
            }
            dbContext.Images.Remove(image);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Image>> GetAll()
        {
            return await dbContext.Images.ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(Guid imageId)
        {
            return await dbContext.Images.FindAsync(imageId);
        }

        public async Task<IEnumerable<Image>> GetByReportIdAsync(Guid reportId)
        {
            return await dbContext.Images
                .Where(i => i.ReportId == reportId)
                .OrderByDescending(i => i.DateCreated)
                .ToListAsync();
        }

        public async Task<Image> Upload(IFormFile file, Image image)
        {
            var imagesFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var guidFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var localPath = Path.Combine(imagesFolder, guidFileName);
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{guidFileName}";
            image.Url = urlPath;
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }
    }
}
