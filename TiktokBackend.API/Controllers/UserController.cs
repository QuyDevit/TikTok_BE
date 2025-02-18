using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiktokBackend.Application.Queries.Users;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IJwtService _jwtService;
        public UserController(ISender sender, IJwtService jwtService)
        {
            _sender = sender;
            _jwtService = jwtService;
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetInfoUserAsync()
        {
            var token = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            var userId = _jwtService.ValidateToken(token);
            var result = await _sender.Send(new GetUserByIdQuery(userId));
            return Ok(result);
        }
    }
}
