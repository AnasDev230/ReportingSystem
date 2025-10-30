using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.ReportType;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTypesController : ControllerBase
    {
        private readonly IReportTypeRepository reportTypeRepository;
        private readonly IMapper mapper;

        public ReportTypesController(IReportTypeRepository reportTypeRepository,IMapper mapper)
        {
            this.reportTypeRepository = reportTypeRepository;
            this.mapper = mapper;
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateReportType([FromBody] CreateReportTypeRequestDto request)
        {
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
        [HttpGet("Departments/{departmentId}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetAllReportTypesByDepartmentId([FromRoute] Guid departmentId)
        {
            return Ok(mapper.Map<List<ReportTypeDto>>(await reportTypeRepository.GetByDepartmentIdAsync(departmentId)));
        }
        [HttpGet("Governorates/{governorateId}")]
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
                return NotFound();
            return Ok(mapper.Map<ReportTypeDto>(reportType));
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateReportType([FromRoute]Guid Id,[FromBody] UpdateReportTypeRequestDto request)
        {
            ReportType reportType = await reportTypeRepository.GetByIdAsync(Id);
            if (reportType == null)
                return NotFound();
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
                return NotFound();
            if(await reportTypeRepository.DeleteAsync(Id))
                return Ok();
            return BadRequest();
        }
    }
}
