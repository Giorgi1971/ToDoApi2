using System;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using TodoApp.Api.Db;
using TodoApp.Api.Db.Entity;
using TodoApp.Api.Models.Requests;
using TodoApp.Api.Repositories;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // ანუ როუთად  კლასის სახელს აიღებს

    public class AuthController : ControllerBase
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISendEmailRequestRepository _sendEmailRequestRepository;


        public AuthController(
            IConfiguration configuration,
            ISendEmailRequestRepository sendEmailRequestRepository,
            UserManager<UserEntity> userManager,
            TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _sendEmailRequestRepository = sendEmailRequestRepository;
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
            entity.UserName = request.Email;
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
        public async Task<IActionResult> RequestPasswordReset([FromBody]RequestPasswordResetRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Use Notr Found");
            // 1 Generate password reset Token
            var token = _userManager.GeneratePasswordResetTokenAsync(user);
            // 2 Insert email into SendEmailRequest table
            var sendEmailRequestEntity = new SendEmailRequestEntity();
            sendEmailRequestEntity.ToAddress = request.Email;
            sendEmailRequestEntity.Status = SendEmailRequestStatus.New;
            sendEmailRequestEntity.CreateAt = DateTime.Now;
            var url = _configuration["PasswordResetUrl"]!
                .Replace("{UserId}", user.Id.ToString())
                .Replace("{token}", token.ToString());
            var resetUrl = $"<a href =\"{url}\">Reset Password</a>";
            sendEmailRequestEntity.Body = $"Plaese, Reset Your Password: {resetUrl}";

            _sendEmailRequestRepository.Insert(sendEmailRequestEntity);
            await _sendEmailRequestRepository.SaveChangesAsync();
            // 3 Return result
            return Ok();
        }

        // TODO: - II ResetPassword
        [HttpPost("reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return NotFound("user Not Found.");
            var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!resetResult.Succeeded)
            {
                var firstError = resetResult.Errors.First();
                return StatusCode(500, firstError.Description);
            }
            // 1 validate Token

            // 2 validate new Password
            // 3 Reset Password
            // 4 Return result
            return Ok();
        }
    }
}
