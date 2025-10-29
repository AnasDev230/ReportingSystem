using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(Guid reportId);
        Task<IEnumerable<Report>> GetByGovernorateIdAsync(Guid governorateId);
        Task<IEnumerable<Report>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Report>> GetByDepartmentIdAsync(Guid departmentId);
        Task<IEnumerable<Report>> GetByReportTypeIdAsync(Guid reportTypeId);
        Task<Report> CreateAsync(Report report);
        Task<Report?> UpdateAsync(Report report);
        Task<bool> DeleteAsync(Guid reportId);

        //Report Updates

        Task<IEnumerable<object>> GetReportsForEmployeeAsync(string userId);
        Task<ReportUpdate> AddReportUpdateAsync(Guid reportId, Guid employeeId, string newStatus, string? comment = null);
    }
}
