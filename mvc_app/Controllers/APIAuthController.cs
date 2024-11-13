using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using mvc_app.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mvc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIAuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public APIAuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        //POST
        //https:localhost:[port]/api/register
        //Register
        [HttpPost("register")]//MapController подключает 
        public async Task<IActionResult> Register([FromBody] mvc_app.Models.RegisterModel registerModel)
        {
            var user = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email, EmailConfirmed=true };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "User registered Successfully" });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        //https:localhost:[port]/api/login
        //Register
        [HttpPost("login")]//MapController подключает 
        public async Task<IActionResult> Login([FromBody] mvc_app.Models.LogginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null)
            {
                return Unauthorized("Invalid login attempt");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new {Token = token });
            }
            else
            {
                return Unauthorized("Invalid login attempt");
            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptior);
            return tokenHandler.WriteToken(token);
        }
    }
}
