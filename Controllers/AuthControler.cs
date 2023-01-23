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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // TODO:Check user credentials...
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound("User not found");

            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isCorrectPassword)
                return BadRequest("Invalid email or password");

            return Ok(_tokenGenerator.Generate(request.Email));
        }

        // TODO:register
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
             var token = await _userManager.GenerateEmailConfirmationTokenAsync(entity);

            return Ok();
        }

        // TODO: - I RegisterPasswordReset
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Use Notr Found");
        // 1 Generate password reset Token
            var token = _userManager.GeneratePasswordResetTokenAsync(user);
        // 2 Insert email into SendEmailRequest table

        // 3 Return result


            return Ok();
        }


        // TODO: - II ResetPassword
        // 1 validate Token
        // 2 validate new Password
        // 3 Reset Password
        // 4 Return result

    }
}
