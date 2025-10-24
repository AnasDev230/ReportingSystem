using ReportingSystem.Models.Domain;

namespace ReportingSystem.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<Employee> CreateAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetAllByDepartmentIdAsync(Guid departmentId);
        Task<IEnumerable<Employee>> GetAllByGovernorateIdAsync(Guid governorateId);
        Task<Employee?> GetByIDAsync(Guid id);
        Task<Employee?> GetByUserIDAsync(string userId);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(Guid id);
    }
}
