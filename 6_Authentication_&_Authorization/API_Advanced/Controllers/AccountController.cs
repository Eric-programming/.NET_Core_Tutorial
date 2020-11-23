using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API_Advanced.Extensions;
using API_Advanced.Models;
using API_Advanced.Models.DTO;
using API_Advanced.Models.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace API_Advanced.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();
            //Add Login Block if email is not confirmed
            if (user.EmailConfirmed == false)
            {
                return Unauthorized("Your email is not confirmed");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (result.IsLockedOut) return Unauthorized("You are locked out! Please try again or reset password");
            if (!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return BadRequest("Email address is in use");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest();
            //Create Token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = token,//Instead of jwt, it is going to be email confirm token
                Email = user.Email,
                Id = user.Id
            };
        }
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<UserDto>> ForgotPassword(string Email)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(Email);
            // If the user is not found OR Email isn't confirmed
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return Unauthorized();
            }
            // Generate the reset password token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = token,//Instead of jwt, it is going to be reset password token
                Email = user.Email,
                Id = user.Id
            };
        }
        [HttpPost("ResetPassword")]

        public async Task<ActionResult> ResetPassword(string userId, ResetPasswordDto resetPasswordDto)
        {
            if (userId == null || resetPasswordDto.Token == null)
            {
                return BadRequest("Invalid Reset data");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Can't find the user");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!result.Succeeded)
            {
                return Unauthorized("Unable to reset password");
            }
            return NoContent();
        }
        [HttpPost("ConfirmEmail")]

        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Invalid Confirmation Link");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Can't find the user");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return Unauthorized("Unable to confirm the email");
            }
            return NoContent();
        }

    }
}