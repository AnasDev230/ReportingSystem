using AutoMapper;
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

        public ReportsController(IReportRepository reportRepository, IMapper mapper)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequestDto request)
        {
            Report report=mapper.Map<Report>(request);
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
        [HttpGet("GetReportsByReportTypeId/{DepartmentId}")]
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
            Report report=await reportRepository.GetByIdAsync(ReportId);
            if (report==null)
                return NotFound();
            mapper.Map(request, report);
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
    }
}
