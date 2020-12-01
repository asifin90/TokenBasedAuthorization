using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Token_based_authentication.Models;

namespace Token_based_authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login(string userId, string password)
        {
            ApplicationUser _user = new ApplicationUser();
            _user.UserName = userId;
            _user.Password = password;
            IActionResult response = Unauthorized();
            var auth = Authenticate(_user);

            if(auth != null)
            {
                var tokenstr = CreateToken(auth);
                response = Ok(new { token = tokenstr });
            }
            return response;
        }

        private ApplicationUser Authenticate(ApplicationUser _user)
        {
            ApplicationUser user = null;
            if(_user.UserName == "ABC" && _user.Password == "123")
            {
                user = new ApplicationUser { UserName = "ABC", Password = "123", Email = "ABC@gmail.com" };
            }
            return user;
        }

        private string CreateToken(ApplicationUser _user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        [Authorize]
        [HttpPost("Message")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Sample Post method for User: " + userName;
        }

        [Authorize]
        [HttpGet("ReadValues")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "ABC", "XYZ", "345" };
        }
    }
}
