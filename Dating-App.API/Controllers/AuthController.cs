using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dating_App.API.Data;
using Dating_App.API.DTOs;
using Dating_App.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dating_App.API.Controllers
{
    [Route("api/[controller]")]
    // allows ASP.NET core to infer the info thats in the data
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // If we do not use [ApiController]
            // if(!ModelState.IsValid)
            //     return BadRequest(ModelState);

            // validate request

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await this.repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await this.repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // Check to see if user is stored within the database
            var userFromRepo = await this.repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // token contains two Claims: user id and username
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // In order to make sure the token is a valid token when it comes back,
            // We need to sign the credentials with our key
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(this.config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Creation of our token: takes our Claims, 24 hr expiration, and our credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                 Subject = new ClaimsIdentity(claims),
                 Expires = DateTime.Now.AddDays(1),
                 SigningCredentials = creds
            };

            // Create our token based on our parameters with tokenDescriptor
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write token in the response back to our client
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}