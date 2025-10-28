using System.Security.Claims;
using AutoMapper;
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

        public ReportsController(IReportRepository reportRepository, IMapper mapper,IEmployeeRepository employeeRepository)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized();
            Report report=mapper.Map<Report>(request);
            report.UserId = userId;
            report.Status = "Pending";
            await reportRepository.CreateAsync(report);
            return Created("",mapper.Map<ReportDto>(report));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await reportRepository.GetAllAsync();
            var reportsDto = mapper.Map<IEnumerable<ReportDto>>(reports);
            return Ok(reportsDto);
        }
        [HttpGet("{ReportId}")]
        public async Task<IActionResult> GetReportById([FromRoute] Guid ReportId)
        {
            Report report= await reportRepository.GetByIdAsync(ReportId);
            if(report==null)
                return NotFound();
            return Ok(mapper.Map<ReportDto>(report));
        }

        [HttpGet("GetReportsByGovernorateId/{GovernorateId}")]
        public async Task<IActionResult> GetReportsByGovernorateId([FromRoute] Guid GovernorateId)
        {
            var reports=await reportRepository.GetByGovernorateIdAsync(GovernorateId);
            if (reports==null)
                return NotFound();
            return Ok(mapper.Map<List<ReportDto>>(reports));
        }
        [HttpGet("GetReportsByDepartmentId/{DepartmentId}")]
        public async Task<IActionResult> GetReportsByDepartmentId([FromRoute] Guid DepartmentId)
        {
            var reports = await reportRepository.GetByDepartmentIdAsync(DepartmentId);
            if (reports == null)
                return NotFound();
            return Ok(mapper.Map<List<ReportDto>>(reports));
        }
        [HttpGet("GetReportsByReportTypeId/{ReportTypeId}")]
        public async Task<IActionResult> GetReportsByReportTypeId([FromRoute] Guid ReportTypeId)
        {
            var reports = await reportRepository.GetByReportTypeIdAsync(ReportTypeId);
            if (reports == null)
                return NotFound(); 
            return Ok(mapper.Map<List<ReportDto>>(reports));
        }
        [HttpPut("{ReportId}")]
        public async Task<IActionResult> UpdateReport([FromRoute] Guid ReportId,[FromBody] UpdateReportRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized();
            Report report=await reportRepository.GetByIdAsync(ReportId);
            if (report==null)
                return NotFound();
            if (report.Status != "Pending")
            {
                return BadRequest();
            }
            mapper.Map(request, report);
            report.UserId = userId;
            report=await reportRepository.UpdateAsync(report);
            return Ok(mapper.Map<ReportDto>(report));
        }
        [HttpDelete("{ReportId}")]
        public async Task<IActionResult> DeleteReport([FromRoute]Guid ReportId)
        {
            Report report=await reportRepository.GetByIdAsync(ReportId);
            if(report==null)
                return NotFound();
            if(await reportRepository.DeleteAsync(ReportId))
                return Ok();
            return BadRequest();

        }



        



        [HttpGet("GetReportsForEmployee")]
        [Authorize(Roles = "Employee")]

        public async Task<IActionResult> GetReportsForEmployee()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            
            var reports=await reportRepository.GetReportsForEmployeeAsync(userId);

            return Ok(mapper.Map<List<ReportDto>>(reports));
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
        "In-Progress",
        "Completed",
        "Rejected"
    };
            if (!allowedStatuses.Contains(request.NewStatus))
            {
                return BadRequest("Invalid status value");
            }
            var reportUpdate=await reportRepository.AddReportUpdateAsync(reportId,employee.EmployeeId,request.NewStatus,request.Comment);
            return Ok(new
            {
                reportUpdate.ReportId,
                reportUpdate.Status
            });
        }

    }
}
