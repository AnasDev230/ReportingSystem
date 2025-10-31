using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;
        public DepartmentsController(IDepartmentRepository departmentRepository,IMapper mapper,IEmployeeRepository employeeRepository)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles ="No Roles For Now")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequestDto request)
        {
            Department department=mapper.Map<Department>(request);
            await departmentRepository.CreateAsync(department);
            return Created("",mapper.Map<DepartmentDto>(department));
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments=await departmentRepository.GetAllAsync();
            return Ok(mapper.Map<List<DepartmentDto>>(departments));    
        }

        [HttpGet("GetByGovernorateId/{governorateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetAllDepartmentsByGovernorateId([FromRoute] Guid governorateId)
        {
            var departments = await departmentRepository.GetAllByGovernorateIdAsync(governorateId);
            if (departments == null)
                return NotFound("Department Not Found!");
            return Ok(mapper.Map<List<DepartmentDto>>(departments));
        }
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] Guid Id)
        {
            var department = await departmentRepository.GetByID(Id);
            if(department == null)
                return NotFound("Department Not Found!");
            return Ok(mapper.Map<DepartmentDto>(department));
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid Id,UpdateDepartmentRequestDto request)
        {
            var department=await departmentRepository.GetByID(Id);
            if(department==null)
                return NotFound("Department not found.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");


            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if(admin.DepartmentId!=department.DepartmentId)
                return Forbid("You do not have permission to access this resource.");



            mapper.Map(request, department);
            department = await departmentRepository.UpdateAsync(department);
        return Ok(mapper.Map<DepartmentDto>(department));
        }
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid Id)
        {
            var department = await departmentRepository.GetByID(Id);
            if (department == null)
                return NotFound("Department Not Found!");



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");


            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if (admin.DepartmentId != department.DepartmentId)
                return Forbid("You do not have permission to access this resource.");







            if (await departmentRepository.DeleteAsync(Id))
                return Ok("Department Deleted Successfully");
            return BadRequest("Something Went Wrong!");

        }
    }
}
