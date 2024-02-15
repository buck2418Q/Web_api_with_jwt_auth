using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_api_with_jwt_auth.Data;
using Web_api_with_jwt_auth.Model;

namespace Web_api_with_jwt_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        IConfiguration _config;
        private readonly Application_db_context _dbContext;
        public UserController(IConfiguration configuration,Application_db_context db_Context)
        {
            _config = configuration;
            _dbContext = db_Context;
            
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody]User user)

        {
            IActionResult response = Unauthorized();
            if (user == null)
            {
                if(user.Name.Equals("admin") && user.Password.Equals("1234"))
                {
                    var issuer = _config["Jwt:Issuer"];
                    var audience = _config["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                    var signingCredentials = new SigningCredentials(
                                            new SymmetricSecurityKey(key),
                                            SecurityAlgorithms.HmacSha512Signature
                                        );
                    var subject = new ClaimsIdentity(new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
    new Claim(JwtRegisteredClaimNames.Email, user.Password),
});
                    var expires = DateTime.UtcNow.AddMinutes(10);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = DateTime.UtcNow.AddMinutes(10),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);
                    return Ok(jwtToken);

                }
            }
            return response;
        }

    }
}
