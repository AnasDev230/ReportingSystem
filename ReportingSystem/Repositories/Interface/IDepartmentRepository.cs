using Microsoft.Extensions.Hosting;
using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IDepartmentRepository
    {
        Task<Department> CreateAsync(Department department);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<IEnumerable<Department>> GetAllByGovernorateIdAsync(Guid GovernorateId);
        Task<Department?> GetByID(Guid id);
        Task<Department> GetByReportTypeIdAsync(Guid reportTypeId);
        Task<Department?> UpdateAsync(Department department);
        Task<bool> DeleteAsync(Guid id);
    }
}
