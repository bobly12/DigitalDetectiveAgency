using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.DTOs; 

namespace DigitalDetectiveAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// POST: api/account/register
        /// Registers a new player in the system.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                DisplayName = registerDto.Email.Split('@')[0], 
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            // Assign default player role
            await _userManager.AddToRoleAsync(user, "Player");

            return Ok(new { message = "Registration successful! You can now log in." });
        }

        /// <summary>
        /// POST: api/account/login
        /// Validates player credentials and starts an authentication session.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password match." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid email or password match." });
            }

            // Establish the active cookie authentication session context
            await _signInManager.SignInAsync(user, isPersistent: true);
            
            return Ok(new UserAuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = "Cookie-Session-Active" 
            });
        }

        /// <summary>
        /// POST: api/account/logout
        /// Destroys the active player authentication session.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
        }
    }
}