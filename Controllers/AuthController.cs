using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using InventoryManager.API.Data.Dto.Auth;

namespace InventoryManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Register user
        /// </summary>
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<UserRegisterDto>> RegisterUser([FromBody] UserRegisterDto model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                return BadRequest(result);
            }

            return Ok(new { Username = user.UserName });
        }

        /// <summary>
        /// User login
        /// </summary>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<UserLoginDto>> Login([FromBody] UserLoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["JWT:ServerSecret"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["JWT:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["JWT:Issuer"],
                  audience: _configuration["JWT:Audience"],
                  expires: DateTime.Now.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo
                  });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Logout user
        /// </summary>
        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
