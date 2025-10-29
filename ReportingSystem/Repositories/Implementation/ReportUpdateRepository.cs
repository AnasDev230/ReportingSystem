using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Repositories.Implementation
{
    public class ReportUpdateRepository : IReportUpdateRepository
    {
        private readonly SystemDbContext dbContext;

        public ReportUpdateRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<ReportUpdate>> GetAllAsync()
        {
            return await dbContext.ReportUpdates
                .Include(r => r.Employee)
                .Include(r => r.Report)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReportUpdate>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await dbContext.ReportUpdates
                .Include(r => r.Report)
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ReportUpdate?> GetByIdAsync(Guid id)
        {
            return await dbContext.ReportUpdates
                .Include(r => r.Employee)
                .Include(r => r.Report)
                .FirstOrDefaultAsync(r => r.ReportUpdateId == id);
        }

        public async Task<IEnumerable<ReportUpdate>> GetByReportIdAsync(Guid reportId)
        {
            return await dbContext.ReportUpdates
                .Include(r => r.Employee)
                .Where(r => r.ReportId == reportId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
