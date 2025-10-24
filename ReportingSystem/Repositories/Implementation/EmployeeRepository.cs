using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SystemDbContext dbContext;
        public EmployeeRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Employee> CreateAsync(Employee employee)
        {
           await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Employee employee= await dbContext.Employees.FindAsync(id);
            if(employee == null)
                return false;
            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await dbContext.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllByDepartmentIdAsync(Guid departmentId)
        {
           return await  dbContext.Employees.Where(x=>x.DepartmentId==departmentId).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllByGovernorateIdAsync(Guid governorateId)
        {
            return await dbContext.Employees.Include(d => d.Department)
                .ThenInclude(g => g.Governorate)
                .Where(x => x.Department.GovernorateId == governorateId).ToArrayAsync();
        }

        public async Task<Employee?> GetByIDAsync(Guid id)
        {
            return await dbContext.Employees.FindAsync(id);
        }

        public async Task<Employee?> GetByUserIDAsync(string userId)
        {
            return await dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }


        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            Employee existingEmployee=await dbContext.Employees.FindAsync(employee.EmployeeId);
            if(existingEmployee == null)
                return null;
            dbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
            await dbContext.SaveChangesAsync();
            return employee;

        }
    }
}
