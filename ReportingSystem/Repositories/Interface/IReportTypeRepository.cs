using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IReportTypeRepository
    {
        Task<ReportType> CreateAsync(ReportType reportType);
        Task<IEnumerable<ReportType>> GetAllAsync();
        Task<ReportType?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReportType>> GetByDepartmentIdAsync(Guid departmentId);
        Task<IEnumerable<ReportType>> GetByGovernorateIdAsync(Guid governorateId);
        Task<ReportType?> UpdateAsync(ReportType reportType);
        Task<bool> DeleteAsync(Guid id);
    }
}
