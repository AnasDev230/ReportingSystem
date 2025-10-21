using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Repositories.Interface;
using ReportingSystem.Models.DTO.Auth;
using Microsoft.EntityFrameworkCore;

namespace ReportingSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
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
                if (request.Roles is not null && request.Roles.Any())
                    identityResult = await userManager.AddToRolesAsync(identityUser, request.Roles);
                if (identityResult.Succeeded)
                {
                    return Ok("User Registered Successfully, pls Login");
                }
            }
            return BadRequest("Something Went Wrong!!");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var user = await userManager.FindByEmailAsync(request.Email);
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == NormalizeToLocalSyrianPhone(request.PhoneNumber));

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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                phoneNumber= user.PhoneNumber
            });
        }

        [HttpDelete("DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
                return Unauthorized();
            var identityUser = await userManager.FindByIdAsync(userID);
            if (identityUser == null)
                return BadRequest("Something Went Wrong!");
            await userManager.DeleteAsync(identityUser);
            return Ok("User Deleted Successfully");

        }
        [HttpPut("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequestDto request)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
                return Unauthorized();
            var identityUser = await userManager.FindByIdAsync(userID);
            if (identityUser == null)
                return BadRequest("Something Went Wrong!");
            var response = await userManager.ChangePasswordAsync(identityUser, request.CurrentPassword, request.NewPassword);
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
