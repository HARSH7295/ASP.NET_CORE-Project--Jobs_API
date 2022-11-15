using JobsAPI.DTOs;
using JobsAPI.DBControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            if(await _repo.UserExists(userForRegisterDTO.email))
            {
                return BadRequest("EmailID is already used, please try different one.!!");
            }
            else
            {
                var createdUser = await _repo.Register(userForRegisterDTO);
                return Ok(createdUser);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.Login(userForLoginDTO);
            if(userFromRepo == null)
            {
                return Unauthorized();
            }
            else
            {
                // secret key :   supersecretkey6519

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Email,userFromRepo.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkey6519"));

                // this takes security key and hash it
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                });
            }
        }
    }
}
