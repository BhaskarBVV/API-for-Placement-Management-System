using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_api.Data;
using System.Linq;
using PMS_api.Model;
using PMS_api.Utility;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace PMS_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    { 
        ApiDbContext _dbC = new ApiDbContext();
        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("/Login")]
        public IActionResult Login([FromBody] User user)
        {
            var currentUser = _dbC.User.FirstOrDefault(x => x.userName == user.userName && x.pass == user.pass);
            if (currentUser == null)
                return BadRequest("Invalid Username or password");

            // if login is succesful then generate the JWT token, by using private IConfiguration _config;

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData, user.userName),
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new {token=jwt}); // send the jwt token in the json format.
        }

    }  
}
