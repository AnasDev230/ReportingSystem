using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IReportUpdateRepository
    {
        Task<IEnumerable<ReportUpdate>> GetAllAsync();
        Task<IEnumerable<ReportUpdate>> GetByReportIdAsync(Guid reportId);
        Task<IEnumerable<ReportUpdate>> GetByEmployeeIdAsync(Guid employeeId);
        Task<ReportUpdate?> GetByIdAsync(Guid id);
    }
}
