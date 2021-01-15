using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Authentication.Jwt;
using System.Threading.Tasks;

namespace SampleWebApp.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(JwtTokenGenerator jwtTokenGenerator, UserManager<IdentityUser> userManager)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Login(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Ok();

            var token = _jwtTokenGenerator.GenerateJwtToken(user);

            return Ok(token);
        }
    }
}