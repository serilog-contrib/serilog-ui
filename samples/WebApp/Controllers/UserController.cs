using Microsoft.AspNetCore.Mvc;
using WebApp.Authentication.Jwt;

namespace WebApp.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController(JwtTokenGenerator jwtTokenGenerator) : ControllerBase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        [HttpGet]
        public ActionResult<string> Login(bool failingToken)
        {
            if (failingToken) return Ok(_jwtTokenGenerator.GenerateInvalidJwtToken());

            var token = _jwtTokenGenerator.GenerateJwtToken();

            return Ok(token);
        }
    }
}