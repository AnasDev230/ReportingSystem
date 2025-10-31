using System.Security.Claims;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingSystem.Models.Domain;
using ReportingSystem.Models.DTO.Auth;
using ReportingSystem.Models.DTO.Employee;
using ReportingSystem.Repositories.Interface;

namespace ReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IGovernorateRepository governorateRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository,IDepartmentRepository departmentRepository,IGovernorateRepository governorateRepository)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.departmentRepository = departmentRepository;
            this.governorateRepository = governorateRepository;
        }





        [HttpGet("GetAllEmployeesByDepartmentId/{departmentId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployeesByDepartmentId([FromRoute] Guid departmentId)
        {

            var department=await departmentRepository.GetByID(departmentId);
            if(department == null)
                return NotFound("Department Not Found!");




            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if (admin.DepartmentId != department.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");


            var employees=await employeeRepository.GetAllByDepartmentIdAsync(department.DepartmentId);

            if (!employees.Any())
                return NotFound("No Employees in this Department!");

            var result = employees.Select(e => new
            {
                EmployeeId = e.EmployeeId,
                DepartmentId = e.DepartmentId,
                DepartmentName=e.Department.Name,
                UserId = e.UserId,
                EmployeeName = e.User.UserName,
                PhoneNumber = e.User.PhoneNumber,
                Email = e.User.Email,
                Governorate=e.Department.Governorate.Name,
            });
            return Ok(result);


        }





        [HttpGet("GetAllEmployeesByGovernorateId/{governorateId}")]
        [Authorize(Roles = "No Roles For Now")]
        public async Task<IActionResult> GetAllEmployeesByGovernorateId([FromRoute] Guid governorateId)
        {

            var governorate = await governorateRepository.GetByID(governorateId);
            if (governorate == null)
                return NotFound("Governorate Not Found!");




            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized("Authentication is required. Please log in again.");

            var admin = await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if (admin.Department.Governorate.GovernorateId != governorate.GovernorateId)
                return Forbid("Super Admins can only perform actions on their own Governorate.");


            var employees = await employeeRepository.GetAllByGovernorateIdAsync(governorate.GovernorateId);

            if (!employees.Any())
                return NotFound("No Employees in this Department!");

            var result = employees.Select(e => new
            {
                EmployeeId = e.EmployeeId,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name,
                UserId = e.UserId,
                EmployeeName = e.User.UserName,
                PhoneNumber = e.User.PhoneNumber,
                Email = e.User.Email,
                Governorate = e.Department.Governorate.Name,
            });
            return Ok(result);




        }















        [HttpPost("CreateEmployee")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
                return Unauthorized();

            var admin= await employeeRepository.GetByUserIDAsync(userId);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");

            if (admin.DepartmentId !=request.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");
            

            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = NormalizeToLocalSyrianPhone(request.PhoneNumber),
            };
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, ["Employee"]);
                if (identityResult.Succeeded)
                {
                    var employee = new Employee
                    {
                        UserId=identityUser.Id,
                        DepartmentId=request.DepartmentId,
                    };
                    employee=await employeeRepository.CreateAsync(employee);
                    return Created("User Registered Successfully, pls Login", mapper.Map<EmployeeDto>(employee));
                }
            }
            return BadRequest(identityResult.Errors);


        }



        [HttpPost("CreateAdmin")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = NormalizeToLocalSyrianPhone(request.PhoneNumber),
            };
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, ["Admin"]);
                if (identityResult.Succeeded)
                {
                    var employee = new Employee
                    {
                        UserId = identityUser.Id,
                        DepartmentId = request.DepartmentId,
                    };
                    employee = await employeeRepository.CreateAsync(employee);
                    return Created("User Registered Successfully, pls Login", mapper.Map<EmployeeDto>(employee));
                }
            }
            return BadRequest(identityResult.Errors);


        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(request.Email);
            
            if (user is not null)
            {
                var PasswordResult = await userManager.CheckPasswordAsync(user, request.Password);
                if (PasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = await tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }

                }
            }
            return BadRequest("Username Or Password Incorrect!!");
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Profile")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Authentication is required. Please log in again.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User Not Found!");
            var employee= await employeeRepository.GetByUserIDAsync(user.Id);
            if (employee == null)
                return NotFound("Employee not found for this user!");


            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                DepartmentId=employee.DepartmentId
            });
        }

        [HttpDelete("DeleteEmployee/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
                return Unauthorized();



            var employee = await employeeRepository.GetByIDAsync(employeeId);
            if (employee == null)
                return NotFound("Employee Not Found!");



            var admin = await employeeRepository.GetByUserIDAsync(userID);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");






            if (admin.DepartmentId != employee.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");



            var user = await userManager.FindByIdAsync(employee.UserId);



            await userManager.DeleteAsync(user);
            return Ok("User Deleted Successfully");

        }


        [HttpPut("ChangePassword/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid employeeId,[FromBody] UpdatePasswordRequestDto request)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
                return Unauthorized("Authentication is required. Please log in again.");

            var employee = await employeeRepository.GetByIDAsync(employeeId);
            if (employee == null)
                return NotFound("Employee Not Found!");


            var admin = await employeeRepository.GetByUserIDAsync(userID);
            if (admin == null)
                return Forbid("You do not have permission to access this resource.");




            if (admin.DepartmentId != employee.DepartmentId)
                return Forbid("Admins can only perform actions on their own department.");

            var user = await userManager.FindByIdAsync(employee.UserId);



            var response = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!response.Succeeded)
                return BadRequest(response.Errors);
            return Ok("Password Changed Successfully");
        }

        public static string NormalizeToLocalSyrianPhone(string phone)
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return null;

                phone = phone.Trim().Replace(" ", "").Replace("-", "");

                if (phone.StartsWith("+"))
                    phone = phone.Substring(1);

                if (phone.StartsWith("9639"))
                    return "0" + phone.Substring(3);

                if (phone.StartsWith("09"))
                    return phone;

                return phone;
            }


        }
    }

