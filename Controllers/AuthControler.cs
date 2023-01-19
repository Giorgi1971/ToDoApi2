using System;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using TodoApp.Api.Db.Entity;
using TodoApp.Api.Models.Requests;


namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // ანუ როუთად  კლასის სახელს აიღებს

    public class AuthController : ControllerBase
    {
        private TokenGenerator _tokenGenerator;
        private UserManager<UserEntity> _userManager;

        public AuthController(
            UserManager<UserEntity> userManager,
            TokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }


        [HttpGet("PingWhite")]

        public string PingPong1()
        {
            return "Pong_White";
        }

        [HttpGet("CreateUser")]

        public string CreateUser()
        {
            return "User Creating....";
        }

        // TODO:register
        // TODO:RegisterPasswordReset
        // TODO:ResetPassword

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // TODO:Check user credentials...
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // User not found
                return NotFound("User not found");
            }
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isCorrectPassword)
            {
                return BadRequest("Invalid email or password");
            }

            return Ok(_tokenGenerator.Generate(request.Email));
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            // Create
            var entity = new UserEntity();
            entity.Email = request.Email;
            var result = await _userManager.CreateAsync(entity, request.Password);

            if (!result.Succeeded)
            {
                var firstError = result.Errors.First();
                return BadRequest(firstError.Description);
            }

            // var token = await _userManager.GeneratePasswordResetTokenAsync(entity);

            return Ok();
        }
    }
}
