using System.Security.Claims;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.DTO.ReportUpdate;
using ReportingSystem.Repositories.Implementation;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportUpdatesController : ControllerBase
    {
        private readonly IReportUpdateRepository reportUpdateRepository;
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;

        public ReportUpdatesController(IReportUpdateRepository reportUpdateRepository, IMapper mapper,IEmployeeRepository employeeRepository)
        {
            this.reportUpdateRepository = reportUpdateRepository;
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet("GetAllReportUpdates")]
        //[Authorize(Roles = "No Roles For Now")]
        public async Task<IActionResult> GetAll()
        {
            var updates = await reportUpdateRepository.GetAllAsync();
            return Ok(updates);
        }



        [HttpGet("GetReportUpdatesByEmployeeId/{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetReportUpdatesByEmployeeId([FromRoute] Guid employeeId)
        {
            var updates = await reportUpdateRepository.GetByEmployeeIdAsync(employeeId);
            if (!updates.Any())
                return NotFound("No updates found for this report.");

            var firstUpdate = updates.First();


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");



            var employee = await employeeRepository.GetByUserIDAsync(userId);
            if (employee == null)
                return Forbid("You do not have permission to access this resource.");


            if (firstUpdate.DepartmentId != employee.DepartmentId)
                return Forbid("You can only access reports in your own department.");






            return Ok(updates);
        }




        [HttpGet("GetReportUpdatesByReportId/{reportId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetReportUpdatesByReportId([FromRoute] Guid reportId)
        {

            var updates = await reportUpdateRepository.GetByReportIdAsync(reportId);
            
            if (!updates.Any())
                return NotFound("No updates found for this report.");

            var firstUpdate = updates.First();


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");



            if (User.IsInRole("User") && firstUpdate.UserId != userId)
            {
                return Forbid("You are not allowed to view updates for another user's report.");
            }


            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var employee = await employeeRepository.GetByUserIDAsync(userId);
                if (employee == null)
                    return Forbid("You do not have permission to access this resource.");

                if(firstUpdate.DepartmentId!=employee.DepartmentId)
                    return Forbid("You can only access reports in your own department.");
            }

            return Ok(updates);
        }
        [HttpGet("GetReportUpdatesById/{Id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetReportUpdateById([FromRoute] Guid Id)
        {


            var update = await reportUpdateRepository.GetByIdAsync(Id);

            if (update == null)
                return NotFound("Report update not found.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            var employee = await employeeRepository.GetByUserIDAsync(userId);
            if (employee == null)
                return Forbid("You do not have permission to access this resource.");


            if (employee.DepartmentId != update.DepartmentId)
                return Forbid("You can only access reports in your own department.");

            return Ok(update);


            
        }
    }
}
