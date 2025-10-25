using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Repositories.Implementation
{
    public class ReportTypeRepository:IReportTypeRepository
    {
        private readonly SystemDbContext dbContext;

        public ReportTypeRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ReportType> CreateAsync(ReportType reportType)
        {
            await dbContext.ReportTypes.AddAsync(reportType);
            await dbContext.SaveChangesAsync();
            return reportType;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await dbContext.ReportTypes.FindAsync(id);
            if (existing == null)
                return false;

            dbContext.ReportTypes.Remove(existing);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<ReportType>> GetAllAsync()
        {
            return await dbContext.ReportTypes
                .Include(rt => rt.Department)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReportType>> GetByDepartmentIdAsync(Guid departmentId)
        {
            return await dbContext.ReportTypes
                .Where(rt => rt.DepartmentId == departmentId)
                .Include(rt => rt.Department)
                .ToListAsync();
        }
        public async Task<IEnumerable<ReportType>> GetByGovernorateIdAsync(Guid governorateId)
        {
            return await dbContext.ReportTypes
                .Include(rt => rt.Department)
                .ThenInclude(d => d.Governorate)
                .Where(rt => rt.Department.GovernorateId == governorateId)
                .ToListAsync();
        }

        public async Task<ReportType?> GetByIdAsync(Guid id)
        {
            return await dbContext.ReportTypes
                .Include(rt => rt.Department)
                .FirstOrDefaultAsync(rt => rt.ReportTypeId == id);
        }

        public async Task<ReportType?> UpdateAsync(ReportType reportType)
        {
            var existing = await dbContext.ReportTypes.FindAsync(reportType.ReportTypeId);
            if (existing == null)
                return null;

            dbContext.Entry(existing).CurrentValues.SetValues(reportType);
            await dbContext.SaveChangesAsync();
            return reportType;
        }
    }
}
