using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.ReportUpdate;
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
        public async Task<IEnumerable<ReportUpdateDto>> GetAllAsync()
        {
            return await dbContext.ReportUpdates
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReportUpdateDto
                {
                    ReportUpdateId = r.ReportUpdateId,
                    ReportId = r.ReportId,
                    Status = r.Status,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    EmployeeId = r.EmployeeId,
                    EmployeeName = r.Employee.User.UserName,
                    DepartmentId = r.Employee.DepartmentId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReportUpdateDto>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await dbContext.ReportUpdates
               .Where(r => r.EmployeeId == employeeId)
               .OrderByDescending(r => r.CreatedAt)
               .Select(r => new ReportUpdateDto
               {
                   ReportUpdateId = r.ReportUpdateId,
                   ReportId = r.ReportId,
                   Status = r.Status,
                   Comment = r.Comment,
                   CreatedAt = r.CreatedAt,
                   EmployeeId = r.EmployeeId,
                   EmployeeName = r.Employee.User.UserName,
                   DepartmentId = r.Employee.DepartmentId
               })
               .ToListAsync();
        }

        public async Task<ReportUpdateDto?> GetByIdAsync(Guid id)
        {
            return await dbContext.ReportUpdates
                 .Where(u => u.ReportUpdateId == id)
                 .Select(r => new ReportUpdateDto
                 {
                     ReportUpdateId = r.ReportUpdateId,
                     ReportId = r.ReportId,
                     Status = r.Status,
                     Comment = r.Comment,
                     CreatedAt = r.CreatedAt,
                     EmployeeId = r.EmployeeId,
                     EmployeeName = r.Employee.User.UserName,
                     DepartmentId = r.Employee.DepartmentId
                 })
                 .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<ReportUpdateByReportIdDto>> GetByReportIdAsync(Guid reportId)
        {
            return await dbContext.ReportUpdates
                .Where(r => r.ReportId == reportId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReportUpdateByReportIdDto
                {
                    ReportUpdateId = r.ReportUpdateId,
                    ReportId = r.ReportId,
                    UserId = r.Report.User.Id,
                    Status = r.Status,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    EmployeeId = r.EmployeeId,
                    EmployeeName = r.Employee.User.UserName,
                    DepartmentId = r.Employee.DepartmentId
                })
                .ToListAsync();
        }
    }
}
