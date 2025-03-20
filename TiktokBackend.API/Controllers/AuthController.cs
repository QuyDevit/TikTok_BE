using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiktokBackend.Application.Commands.Auths;
using TiktokBackend.Application.Payloads;

namespace TiktokBackend.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        public AuthController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost("register/otp")]
        public async Task<IActionResult> RegisterWithOtpAsync([FromBody] RegisterRequest.RequestOtp requestOtp)
        {
            var result = await _sender.Send(new RegisterWithOtpCommand(requestOtp));
            return Ok(result);
        }
        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterRequest.RegisterUser registerUser)
        {
            var result = await _sender.Send(new RegisterUserCommand(registerUser));

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult>LoginAsync([FromBody] LoginCommand request)
        {
            var result = await _sender.Send(request);

            return Ok(result);
        }
        [HttpPost("login/otp")]
        public async Task<IActionResult> LoginWithOtpAsync([FromBody] LoginWithOtpCommand requestOtp)
        {
            var result = await _sender.Send(requestOtp);
            return Ok(result);
        }   
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }
            var result = await _sender.Send(new LogoutCommand(userId));

            return Ok(result);
        }
    }
}
