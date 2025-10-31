using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.ReportUpdate;

namespace ReportingSystem.Repositories.Interface
{
    public interface IReportUpdateRepository
    {
        Task<IEnumerable<ReportUpdateDto>> GetAllAsync();
        Task<IEnumerable<ReportUpdateByReportIdDto>> GetByReportIdAsync(Guid reportId);
        Task<IEnumerable<ReportUpdateDto>> GetByEmployeeIdAsync(Guid employeeId);
        Task<ReportUpdateDto?> GetByIdAsync(Guid id);
    }
}
