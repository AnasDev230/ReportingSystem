using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IGovernorateRepository
    {
        Task<IEnumerable<Governorate>> GetAllAsync();
        Task<Governorate?> GetByID(Guid id);
    }
}
