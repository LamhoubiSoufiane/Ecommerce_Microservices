using AuthentificationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AuthentificationService.Interfaces;

namespace AuthentificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenBlacklistService tokenBlacklistService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenBlacklistService = tokenBlacklistService;
        }

        [HttpGet]
        [Route("users")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users
                    .Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.EmailConfirmed,
                        u.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(u).Result
                    })
                    .ToListAsync();

                return Ok(new { Status = "Success", Users = users });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "An error occurred while retrieving users: " + ex.Message });
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username) 
                   ?? await _userManager.FindByEmailAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = new
                    {
                        user.UserName,
                        user.Email,
                        Roles = userRoles
                    }
                });
            }
            return Unauthorized(new { Message = "Invalid username/email or password" });
        }

        [HttpPost]
        [Route("register")]
        //[AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status400BadRequest, 
                        new Response { Status = "Error", Message = "Username already exists!" });

                var emailExists = await _userManager.FindByEmailAsync(model.Email);
                if (emailExists != null)
                    return StatusCode(StatusCodes.Status400BadRequest, 
                        new Response { Status = "Error", Message = "Email already exists!" });

                /*IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };*/
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return StatusCode(StatusCodes.Status400BadRequest, 
                        new Response { 
                            Status = "Error", 
                            Message = "User creation failed! Errors: " + string.Join(", ", errors)
                        });
                }

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Error", Message = "An error occurred while creating the user: " + ex.Message });
            }
        }
        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Récupérer le token depuis l'en-tête de la requête
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
               
            // Décoder le token pour obtenir la date d'expiration
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo - DateTime.UtcNow;

            // Ajouter le token à la liste noire
            await _tokenBlacklistService.AddToBlacklistAsync(token, expiration);

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
