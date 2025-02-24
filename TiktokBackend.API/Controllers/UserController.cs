using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiktokBackend.API.Filters;
using TiktokBackend.Application.Commands.Users;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Application.Queries.Users;

namespace TiktokBackend.API.Controllers
{
    [CustomAuthorize("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetInfoUserAsync()
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }

            var result = await _sender.Send(new GetUserByIdQuery(userId));
            return Ok(result);
        }
        [HttpPatch("updatename")]
        public async Task<IActionResult> UpdateNickName([FromBody] UpdateInfoRequest.RequestNickname request)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }

            var result = await _sender.Send(new UpdateUserNameByIdCommand(userId, request.Nickname));
            return Ok(result);
        }
    }
}
