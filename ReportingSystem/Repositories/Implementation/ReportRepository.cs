using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ReportingSystem.Repositories.Implementation
{
    public class ReportRepository : IReportRepository
    {
        private readonly SystemDbContext dbContext;

        public ReportRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ReportUpdate> AddReportUpdateAsync(Guid reportId, Guid employeeId, string newStatus, string? comment = null)
        {
            var report = await dbContext.Reports.FindAsync(reportId);
            if (report == null)
                throw new Exception("Report not found");


            report.Status = newStatus;
            report.UpdatedAt = DateTime.Now;

            var reportUpdate = new ReportUpdate
            {
                ReportId = reportId,
                EmployeeId = employeeId,
                Status = newStatus,
                Comment = comment,
            };
            await dbContext.ReportUpdates.AddAsync(reportUpdate);
            await dbContext.SaveChangesAsync();
            return reportUpdate;
        }

        public async Task<Report> CreateAsync(Report report)
        {
            await dbContext.Reports.AddAsync(report);
            await dbContext.SaveChangesAsync();
            return report;
        }

        public async Task<bool> DeleteAsync(Guid reportId)
        {
            var existingReport = await dbContext.Reports.FindAsync(reportId);
            if (existingReport == null)
                return false;

            dbContext.Reports.Remove(existingReport);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await dbContext.Reports
                .Include(r => r.ReportType)
                .Include(r => r.Governorate)
                .Include(r => r.ReportType.Department)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByDepartmentIdAsync(Guid departmentId)
        {
            return await dbContext.Reports
                .Include(r => r.ReportType)
                .Include(r => r.Governorate)
                .Include(r => r.User)
                .Where(r => r.ReportType.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByGovernorateIdAsync(Guid governorateId)
        {
            return await dbContext.Reports
                .Include(r => r.ReportType)
                .Include(r => r.ReportType.Department)
                .Include(r => r.User)
                .Where(r => r.GovernorateId == governorateId)
                .ToListAsync();
        }
        public async Task<Report?> GetByIdAsync(Guid reportId)
        {
            return await dbContext.Reports
                .Include(r => r.ReportType)
                .Include(r => r.Governorate)
                .Include(r => r.ReportType.Department)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task<IEnumerable<Report>> GetByReportTypeIdAsync(Guid reportTypeId)
        {
            return await dbContext.Reports
                .Include(r => r.Governorate)
                .Include(r => r.ReportType.Department)
                .Include(r => r.User)
                .Where(r => r.ReportTypeId == reportTypeId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Report>> GetByUserIdAsync(string userId)
        {
            return await dbContext.Reports
                .Include(r => r.ReportType)
                .Include(r => r.Governorate)
                .Include(r => r.ReportType.Department)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetReportsForEmployeeAsync(string userId)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(r => r.UserId == userId);
            if (employee == null)
                return Enumerable.Empty<Report>();


            var reports= await dbContext.Reports.Where(x => x.ReportType.DepartmentId == employee.DepartmentId)
                .Select(r => new
                {
                    r.ReportId,
                    r.Title,
                    r.Description,
                    r.Status,
                    r.CreatedAt,
                    r.Longitude,
                    r.Latitude,
                    r.Address,
                    ReportTypeName = r.ReportType.ReportTypeName,
                    UserPhone = r.User.PhoneNumber,
                    UserName=r.User.UserName
                }).ToListAsync();


            return reports;
        }

        public async Task<Report?> UpdateAsync(Report report)
        {
            var existingReport = await dbContext.Reports.FindAsync(report.ReportId);
            if (existingReport == null)
                return null;

            dbContext.Entry(existingReport).CurrentValues.SetValues(report);
            await dbContext.SaveChangesAsync();
            return existingReport;
        }

    }
}
