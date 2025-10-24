using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReportingSystem.Models.Domain;

namespace ReportingSystem.Data
{
    public class SystemDbContext: IdentityDbContext<IdentityUser>
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            var userRoleId = "c8dc4e7d-ed6b-4f4b-a456-45028a4018cb";
            var EmployeeRoleId = "2665a92d-c8f7-4a75-8d69-d1175b74a67e";
            var adminRoleId = "989c6b97-24ae-402b-b2e7-e74d156747c7";
            var Roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = userRoleId,
            ConcurrencyStamp = userRoleId,
            Name = "User",
            NormalizedName = "USER"
        },
        new IdentityRole
        {
            Id = EmployeeRoleId,
            ConcurrencyStamp = EmployeeRoleId,
            Name = "Employee",
            NormalizedName = "EMPLOYEE"
        },
         new IdentityRole
        {
            Id = adminRoleId,
            ConcurrencyStamp = adminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        },

    };
            modelBuilder.Entity<IdentityRole>().HasData(Roles);

            modelBuilder.Entity<Governorate>()
                .HasMany(g => g.Departments)
                .WithOne(d => d.Governorate)
                .HasForeignKey(d => d.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
