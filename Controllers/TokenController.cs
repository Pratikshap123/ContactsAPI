using ContactsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        IConfiguration configuration;
        public TokenController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody] Token user)
        {
            IActionResult response = Unauthorized();
            if (user != null)
            {
                if (user.Name.Equals("rohitkdulani@gmail.com") && user.Password.Equals("Password"))
                {
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var signingcredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);


                    var subject = new ClaimsIdentity(new[]
                    {

                    new Claim(JwtRegisteredClaimNames.Sub as string,user.Name),
                    new Claim(JwtRegisteredClaimNames.Email as string,user.Name)


                });
                    var expires = DateTime.Now.AddMinutes(10);
                    var TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingcredentials
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(TokenDescriptor);
                    var jwttoken = tokenHandler.WriteToken(token);
                    return Ok(jwttoken);
                }
            }
            return response;
        }
    }
}
