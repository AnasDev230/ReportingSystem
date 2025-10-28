using Microsoft.AspNetCore.Identity;

namespace ReportingSystem.Models.Domain
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<ReportUpdate> ReportUpdates { get; set; }
    }
}
