using core.DTOs.Accounts;
using core.Interfaces;
using core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace core.Controllers
{
    [Route("api/accounts")]
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        private readonly ITokenService _tokenService = tokenService;

        private readonly SignInManager<AppUser> _signInManager = signInManager;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = request.Username,
                    Email = request.Email,
                };
                var createdUser = await _userManager.CreateAsync(appUser, request.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new RegisterResponse
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                    }

                    return StatusCode(403, roleResult.Errors);
                }

                return StatusCode(500, createdUser.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == request.UserName.ToLower());
            if (account == null) return Unauthorized("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(account, request.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid username or password");

            return Ok(
                new LoginResponse
                {
                    UserName = account.UserName,
                    Email = account.Email,
                    Token = _tokenService.CreateToken(account)
                }
            );
        }
    }
}