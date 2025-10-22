using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Department;
using ReportingSystem.Repositories.Implementation;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDepartmentRepository departmentRepository;
        public DepartmentsController(IDepartmentRepository departmentRepository,IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequestDto request)
        {
            Department department=mapper.Map<Department>(request);
            await departmentRepository.CreateAsync(department);
            return Created("",mapper.Map<DepartmentDto>(department));
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments=await departmentRepository.GetAllAsync();
            return Ok(mapper.Map<List<DepartmentDto>>(departments));    
        }

        [HttpGet("GetByGovernorateId/{governorateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDepartmentsByGovernorateId([FromRoute] Guid governorateId)
        {
            var departments = await departmentRepository.GetAllByGovernorateIdAsync(governorateId);
            if (departments == null)
                return NotFound();
            return Ok(mapper.Map<List<DepartmentDto>>(departments));
        }
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartmentById([FromRoute] Guid Id)
        {
            var department = await departmentRepository.GetByID(Id);
            if(department == null)
                return NotFound();
            return Ok(mapper.Map<DepartmentDto>(department));
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid Id,UpdateDepartmentRequestDto request)
        {
            var department=await departmentRepository.GetByID(Id);
            if(department==null)
                return NotFound();
            mapper.Map(request, department);
            department = await departmentRepository.UpdateAsync(department);
        return Ok(mapper.Map<DepartmentDto>(department));
        }
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid Id)
        {
            var department = await departmentRepository.GetByID(Id);
            if (department == null)
                return NotFound();
            if(await departmentRepository.DeleteAsync(Id))
                return Ok();
            return BadRequest();

        }
    }
}
