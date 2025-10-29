using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IReportUpdateRepository
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<IEnumerable<object>> GetByReportIdAsync(Guid reportId);
        Task<IEnumerable<object>> GetByEmployeeIdAsync(Guid employeeId);
        Task<object?> GetByIdAsync(Guid id);
    }
}
