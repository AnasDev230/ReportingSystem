using System;
using System.Security.Claims;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Report;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository reportRepository;
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IReportTypeRepository reportTypeRepository;

        public ReportsController(IReportRepository reportRepository, IMapper mapper,IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository,IReportTypeRepository reportTypeRepository)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;
            this.reportTypeRepository = reportTypeRepository;
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");


            Report report=mapper.Map<Report>(request);
            report.UserId = userId;
            report.Status = "Pending";
            await reportRepository.CreateAsync(report);
            return Created("",mapper.Map<ReportDto>(report));
        }



        [HttpGet]
        //[Authorize(Roles = "No Roles For Now")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await reportRepository.GetAllAsync();
            var reportsDto = mapper.Map<IEnumerable<ReportDto>>(reports);
            return Ok(reportsDto);
        }



        [HttpGet("{ReportId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetReportById([FromRoute] Guid ReportId)
        {
            Report report= await reportRepository.GetByIdAsync(ReportId);

            if(report==null)
                return NotFound();


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");


            if (User.IsInRole("User"))
            {
                if (report.UserId != userId)
                    return Forbid("Access denied. You can only view your own reports.");
            }


            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var employee = await employeeRepository.GetByUserIDAsync(userId);
                if (employee == null)
                    return Forbid("You do not have permission to access this resource.");

                if (report.ReportType.DepartmentId != employee.DepartmentId)
                    return Forbid("You can only access reports in your own department.");
            }


            return Ok(mapper.Map<ReportDto>(report));
        }





        [HttpGet("GetReportsByGovernorateId/{GovernorateId}")]
        //[Authorize(Roles = "No Roles For Now")]
        public async Task<IActionResult> GetReportsByGovernorateId([FromRoute] Guid GovernorateId)
        {
            var reports=await reportRepository.GetByGovernorateIdAsync(GovernorateId);
            if (!reports.Any())
                return NotFound("No Reports For this Governorate!");
            return Ok(mapper.Map<List<ReportDto>>(reports));
        }





        [HttpGet("GetReportsByDepartmentId/{DepartmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReportsByDepartmentId([FromRoute] Guid DepartmentId)
        {

            var department=await departmentRepository.GetByID(DepartmentId);
            if(department==null)
                return NotFound("Department Not Found!");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");





            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            
            if (admin.DepartmentId != department.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");



            var reports = await reportRepository.GetByDepartmentIdAsync(DepartmentId);
            if (!reports.Any())
                return NotFound("No Reports For this Department!");
            return Ok(mapper.Map<List<ReportDto>>(reports));
        }







        [HttpGet("GetReportsByReportTypeId/{ReportTypeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReportsByReportTypeId([FromRoute] Guid ReportTypeId)
        {

            var reportType=await reportTypeRepository.GetByIdAsync(ReportTypeId);
            if (reportType == null)
                return NotFound("Report Type Not Found!");




            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");



            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");




            var department=await departmentRepository.GetByReportTypeIdAsync(ReportTypeId);


            if (admin.DepartmentId != department.DepartmentId)
                return Forbid("You can only access reports in your own department.");




            var reports = await reportRepository.GetByReportTypeIdAsync(ReportTypeId);
            if (!reports.Any())
                return NotFound("No Reports For this Report Type!"); 
            return Ok(mapper.Map<List<ReportDto>>(reports));
        }


        [HttpGet("GetReportsForUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetReportsForUser([FromQuery] string? status, [FromQuery] bool sortByNewest = true)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            var allowedStatuses = new List<string>
              {
          "Pending",
        "UnderReview",
        "In-Progress",
        "Completed",
        "Rejected"
    };
            if (!string.IsNullOrEmpty(status) && !allowedStatuses.Contains(status))
            {
                return BadRequest("Invalid status value");
            }


            var reports = await reportRepository.GetByUserIdAsync(userId,status,sortByNewest);
            if (!reports.Any())
                return NotFound("No Reports For this User!");





            return Ok(mapper.Map<List<ReportDto>>(reports));
        }












        [HttpPut("{ReportId}")]
        [Authorize(Roles ="User,Employee")]
        public async Task<IActionResult> UpdateReport([FromRoute] Guid ReportId,[FromBody] UpdateReportRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized();
            Report report=await reportRepository.GetByIdAsync(ReportId);
            if (report==null)
                return NotFound("Report Not Found!");

            mapper.Map(request, report);
            if (User.IsInRole("User"))
            {

                if (report.Status != "Pending")
                {
                    return BadRequest("You cannot update this report because its status is not Pending.");
                }
                if (report.UserId != userId)
                    return Forbid("You cannot update this report because it does not belong to you");

            }

            report=await reportRepository.UpdateAsync(report);
            return Ok(mapper.Map<ReportDto>(report));
        }



        [HttpDelete("{ReportId}")]
        [Authorize(Roles ="User,Employee")]
        public async Task<IActionResult> DeleteReport([FromRoute]Guid ReportId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized();

            Report report=await reportRepository.GetByIdAsync(ReportId);
            if(report==null)
                return NotFound();

            if (User.IsInRole("User"))
            {
                if (!string.Equals(report.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("You cannot delete this report because its status is not Pending.");
            }
            
            if (await reportRepository.DeleteAsync(ReportId))
                return Ok();
            return BadRequest("Failed to delete report.");

        }


        [HttpGet("GetReportsForEmployee")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetReportsForEmployee([FromQuery] string? status,[FromQuery] bool sortByNewest = true)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            var employee = await employeeRepository.GetByUserIDAsync(userId);
            if (employee == null)
                return Forbid("You do not have permission to access this resource.");

            var allowedStatuses = new List<string>
              {
          "Pending",
        "UnderReview",
        "In-Progress",
        "Completed",
        "Rejected"
    };
            if (!string.IsNullOrEmpty(status) && !allowedStatuses.Contains(status))
            {
                return BadRequest("Invalid status value");
            }




            var reports =await reportRepository.GetReportsForEmployeeAsync(userId,status,sortByNewest);

            if (!reports.Any())
                return NotFound("No Reports For this Department!");

            return Ok(reports);
        }
        




        [HttpPost("UpdateReportStatus/{reportId}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateReportStatus([FromRoute]Guid reportId, [FromBody] UpdateReportStatusRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var employee=await employeeRepository.GetByUserIDAsync(userId);
            if(employee==null)
                return NotFound("Employee Not Found!!");

            var allowedStatuses = new List<string>
              {
          "Pending",
        "UnderReview",
        "In-Progress",
        "Completed",
        "Rejected"
    };




            var report = await reportRepository.GetByIdAsync(reportId);
            if (report == null)
                return NotFound("Report not found.");



            if(employee.DepartmentId!=report.ReportType.DepartmentId)
                return Forbid("Employees can only perform actions on their own department.");








            var currentStatus=report.Status;


            if (!allowedStatuses.Contains(currentStatus))
            {
                return BadRequest("Invalid status value");
            }





            if (currentStatus == "Completed" || currentStatus == "Rejected")
                return BadRequest("This report cannot be updated anymore.");







            var newStatus = "";

            if (currentStatus == "Pending")
                newStatus = "UnderReview";
            if (currentStatus == "UnderReview")
                newStatus = "In-Progress";
            if (currentStatus == "In-Progress")
                newStatus = "Completed";

            var reportUpdate =await reportRepository.AddReportUpdateAsync(reportId,employee.EmployeeId,newStatus,request.Comment);
            return Ok(new
            {
                reportUpdate.ReportId,
                reportUpdate.Status
            });


        }



        [HttpPost("RejectReport/{reportId}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> RejectReport([FromRoute] Guid reportId, [FromBody] UpdateReportStatusRequestDto request)
        {


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var employee = await employeeRepository.GetByUserIDAsync(userId);
            if (employee == null)
                return NotFound("Employee Not Found!!");


            var report = await reportRepository.GetByIdAsync(reportId);
            if (report == null)
                return NotFound("Report not found.");

            if (employee.DepartmentId != report.ReportType.DepartmentId)
                return Forbid("Employees can only perform actions on their own department.");


            var currentStatus = report.Status;
            if (currentStatus == "Rejected" || currentStatus == "Completed" || currentStatus=="In-Progress")
                return BadRequest("This report cannot be rejected anymore.");


            var reportUpdate = await reportRepository.AddReportUpdateAsync(reportId, employee.EmployeeId, "Rejected", request.Comment);

            return Ok(new
            {
                reportUpdate.ReportId,
                reportUpdate.Status
            });
        }



        }
}
