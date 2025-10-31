using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.ReportType;
using ReportingSystem.Repositories.Implementation;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTypesController : ControllerBase
    {
        private readonly IReportTypeRepository reportTypeRepository;
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;

        public ReportTypesController(IReportTypeRepository reportTypeRepository,IMapper mapper,IEmployeeRepository employeeRepository)
        {
            this.reportTypeRepository = reportTypeRepository;
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }


        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateReportType([FromBody] CreateReportTypeRequestDto request)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");



            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if (admin.DepartmentId != request.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");


            ReportType reportType = mapper.Map<ReportType>(request);
            await reportTypeRepository.CreateAsync(reportType);
            return Created("",mapper.Map<ReportTypeDto>(reportType));
        }


        [HttpGet]
        [Authorize(Roles ="No Roles For Now")]
        public async Task<IActionResult> GetAllReportTypes()
        {
            return Ok(mapper.Map<List<ReportTypeDto>>(await reportTypeRepository.GetAllAsync()));
        }



        [HttpGet("GetAllReportTypesByDepartmentId/{departmentId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetAllReportTypesByDepartmentId([FromRoute] Guid departmentId)
        {
            return Ok(mapper.Map<List<ReportTypeDto>>(await reportTypeRepository.GetByDepartmentIdAsync(departmentId)));
        }


        [HttpGet("GetAllReportTypesByGovernorateId/{governorateId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetAllReportTypesByGovernorateId([FromRoute] Guid governorateId)
        {
            return Ok(mapper.Map<List<ReportTypeDto>>(await reportTypeRepository.GetByGovernorateIdAsync(governorateId)));
        }


        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            ReportType reportType=await reportTypeRepository.GetByIdAsync(Id);
            if(reportType == null)  
                return NotFound("Report Type Not Found!");
            return Ok(mapper.Map<ReportTypeDto>(reportType));
        }


        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateReportType([FromRoute]Guid Id,[FromBody] UpdateReportTypeRequestDto request)
        {
            ReportType reportType = await reportTypeRepository.GetByIdAsync(Id);
            if (reportType == null)
                return NotFound("Report Type Not Found!");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");



            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if (admin.DepartmentId != reportType.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");



            mapper.Map(request, reportType);
            reportType=await reportTypeRepository.UpdateAsync(reportType);
            return Ok(mapper.Map<ReportTypeDto>(reportType));
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReportType([FromRoute] Guid Id)
        {


            ReportType reportType = await reportTypeRepository.GetByIdAsync(Id);
            if (reportType == null)
                return NotFound("Report Type Not Found!");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");



            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if(admin.DepartmentId!=reportType.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");


            if (await reportTypeRepository.DeleteAsync(Id))
                return Ok("Report Type Deleted Successfully");


            return BadRequest("Something Went Wrong!");
        }
    }
}
