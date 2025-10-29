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
        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await dbContext.ReportUpdates
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    ReportUpdateId = r.ReportUpdateId,
                    ReportId = r.ReportId,
                    Status = r.Status,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    EmployeeName = r.Employee.User.UserName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await dbContext.ReportUpdates
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    ReportUpdateId = r.ReportUpdateId,
                    ReportId = r.ReportId,
                    Status = r.Status,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    EmployeeName = r.Employee.User.UserName
                }
                )
                .ToListAsync();
        }

        public async Task<object?> GetByIdAsync(Guid id)
        {
            return await dbContext.ReportUpdates.Where(u=>u.ReportUpdateId == id)
                .Select(r => new
                {
                    ReportUpdateId = r.ReportUpdateId,
                    ReportId = r.ReportId,
                    Status = r.Status,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    EmployeeName = r.Employee.User.UserName
                }
                ).ToListAsync();
                
                
        }

        public async Task<IEnumerable<object>> GetByReportIdAsync(Guid reportId)
        {
            return await dbContext.ReportUpdates
                .Where(r => r.ReportId == reportId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    ReportUpdateId = r.ReportUpdateId,
                    ReportId = r.ReportId,
                    Status = r.Status,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    EmployeeName = r.Employee.User.UserName
                }
                )
                .ToListAsync();
        }
    }
}
