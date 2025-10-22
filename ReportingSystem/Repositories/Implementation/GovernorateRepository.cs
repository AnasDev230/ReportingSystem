using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Models.Domain;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Repositories.Implementation
{
    public class GovernorateRepository : IGovernorateRepository
    {
        private readonly SystemDbContext dbContext;
        public GovernorateRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Governorate>> GetAllAsync()
        {
            return await dbContext.Governorates.ToListAsync();
        }

        public async Task<Governorate?> GetByID(Guid id)
        {
            return await dbContext.Governorates.FindAsync(id);
        }
    }
}
