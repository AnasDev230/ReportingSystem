using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(IFormFile file, Image image);
        Task<IEnumerable<Image>> GetByReportIdAsync(Guid reportId);
        Task<IEnumerable<Image>> GetAll();
        Task<Image?> GetByIdAsync(Guid imageId);
        Task<bool> DeleteAsync(Guid imageId);
    }
}
