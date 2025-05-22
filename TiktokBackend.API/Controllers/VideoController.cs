
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiktokBackend.API.Filters;
using TiktokBackend.Application.Commands.Videos;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Application.Queries.Users;
using TiktokBackend.Application.Queries.Videos;
using static TiktokBackend.Application.Payloads.VideoRequest;

namespace TiktokBackend.API.Controllers
{
    [CustomAuthorize("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : Controller
    {
        private readonly ISender _sender;
        public VideoController(ISender sender)
        {
            _sender = sender;
        }
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchVideosByKeywordAsync(string q, string type = "less", int page = 1, int loadMore = 0)
        {
            var result = await _sender.Send(new GetListVideoByKeywordQuery(q, type, page, loadMore));
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetVideosAsync(string type = "for-you", int page = 1, int loadMore = 0)
        {
            Guid? userId = null;
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) &&
                 Guid.TryParse(userIdObj?.ToString(), out var parsedUserId))
            {
                userId = parsedUserId;
            }
            var result = await _sender.Send(new GetListVideoQuery(type, page, loadMore, userId));
            return Ok(result);
        }
        [HttpPost("like")]
        public async Task<IActionResult> LikeVideoByIdAsync([FromBody] VideoRequest.LikeVideoId rq)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }
            var result = await _sender.Send(new LikeVideoCommand(rq.VideoId, userId));
            return Ok(result);
        }
        [HttpPost("upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
        public async Task<IActionResult> UploadVideoAsync([FromForm] List <IFormFile> files, [FromForm] string description, [FromForm] string viewable, [FromForm] string[] allows)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj == null || !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                return Unauthorized(new { message = "Không tìm thấy người dùng!" });
            }
            byte[] videoBytes = null;
            byte[] thumbBytes = null;
            string? originalFileName = null;
            if (files[0] != null)
            {
                originalFileName = files[0].FileName;
                using (var memoryStream = new MemoryStream())
                {
                    await files[0].CopyToAsync(memoryStream);
                    videoBytes = memoryStream.ToArray();
                }
            }
            if (files[1] != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await files[1].CopyToAsync(memoryStream);
                    thumbBytes = memoryStream.ToArray();
                }
            }
            VideoRequest.CreateVideo videoRequest = new VideoRequest.CreateVideo
            {
                UserId = userId,
                Allows = allows,
                Description = description,
                Viewable = viewable,
                Video = videoBytes,
                Thumb = thumbBytes,
                OriginalFileName = originalFileName
            };
            var result = await _sender.Send(new PostVideoCommand(videoRequest));
            return Ok(result);
        }
    }
}
