using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Governorate;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernoratesController : ControllerBase
    {
        private readonly IGovernorateRepository governorateRepository;
        private readonly IMapper mapper;

        public GovernoratesController(IGovernorateRepository governorateRepository,IMapper mapper)
        {
            this.governorateRepository = governorateRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGovernorates()
        {
            var Governorrates=await governorateRepository.GetAllAsync();
            return Ok(mapper.Map<List<GovernorateDto>>(Governorrates));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGovernorateById([FromRoute] Guid Id)
        {
            return Ok(mapper.Map<GovernorateDto>(await governorateRepository.GetByID(Id)));
        }
    }
}
