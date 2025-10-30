using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.DTO.ReportUpdate;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportUpdatesController : ControllerBase
    {
        private readonly IReportUpdateRepository reportUpdateRepository;
        private readonly IMapper mapper;
        public ReportUpdatesController(IReportUpdateRepository reportUpdateRepository, IMapper mapper)
        {
            this.reportUpdateRepository = reportUpdateRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "No Roles For Now")]
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
            return Ok(updates);
        }

        [HttpGet("GetReportUpdatesByReportId/{reportId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetReportUpdatesByReportId([FromRoute] Guid reportId)
        {
            var updates = await reportUpdateRepository.GetByReportIdAsync(reportId);
            return Ok(updates);
        }
        [HttpGet("GetReportUpdatesById/{Id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetReportUpdatesById([FromRoute] Guid Id)
        {
            var updates = await reportUpdateRepository.GetByIdAsync(Id);
            return Ok(updates);
        }
    }
}
