using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Repositories.Implementation
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly SystemDbContext dbContext;
        public DepartmentRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Department> CreateAsync(Department department)
        {
            await dbContext.Departments.AddAsync(department);
            await dbContext.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Department department= await dbContext.Departments.FindAsync(id);
            if(department == null)
                return false;
             dbContext.Departments.Remove(department);
             await dbContext.SaveChangesAsync(); 
            return true;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await dbContext.Departments.ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllByGovernorateIdAsync(Guid GovernorateId)
        {
            return await dbContext.Departments.Where(x=>x.GovernorateId==GovernorateId).ToListAsync();
        }

        public async Task<Department?> GetByID(Guid id)
        {
            return await dbContext.Departments.FindAsync(id);
        }

        public async Task<Department?> UpdateAsync(Department department)
        {
            Department existingDepartment = await dbContext.Departments.FindAsync(department.DepartmentId);
            if (department == null)
                return null;
            dbContext.Entry(existingDepartment).CurrentValues.SetValues(department);
            await dbContext.SaveChangesAsync();
            return department;

        }
    }
}
