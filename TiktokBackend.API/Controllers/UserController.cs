using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetInfoUserByNickNameAsync(string nickname)
        {
            Guid? userId = null;
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) &&
                 Guid.TryParse(userIdObj?.ToString(), out var parsedUserId))
            {
                userId = parsedUserId;
            }
            var result = await _sender.Send(new GetUserByNickNameQuery(nickname, userId));
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchInfoUserByKeywordAsync(string q, string type = "less", int page = 1, int loadMore = 0)
        {
            var result = await _sender.Send(new GetListUserByKeywordQuery(q,type,page,loadMore));
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("suggested")]
        public async Task<IActionResult> SuggestUserAsync(int page = 1, int per_page = 10)
        {
            var result = await _sender.Send(new GetListUserSuggestQuery (page, per_page));
            return Ok(result);
        }
        [HttpPost("follow")]
        public async Task<IActionResult> FollowAsync([FromBody] UserRequest.FollowUserId request)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }
            var result = await _sender.Send(new FollowUserByIdCommand(userId, request.UserId));
            return Ok(result);
        }
        [HttpPatch("updatename")]
        public async Task<IActionResult> UpdateNickNameAsync([FromBody] UpdateInfoRequest.RequestNickname request)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }

            var result = await _sender.Send(new UpdateUserNameByIdCommand(userId, request.Nickname));
            return Ok(result);
        }
        [HttpPatch("updateinfo")]
        public async Task<IActionResult> UpdateInfoAsync([FromForm] IFormFile? avatar, [FromForm] string fullname, [FromForm] string nickname, [FromForm] string? bio)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }
            byte[] avatarBytes = null;
            if (avatar != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await avatar.CopyToAsync(memoryStream);
                    avatarBytes = memoryStream.ToArray(); 
                }
            }
            var request = new UpdateInfoRequest.RequestInfo
            {
                UserId = userId,
                Fullname = fullname,
                Nickname = nickname,
                Bio = bio ==null ? "":bio,
                Avatar = avatarBytes
            };
            var result = await _sender.Send(new UpdateUserInfoByIdCommand(request));
            return Ok(result);
        }
    }
}
