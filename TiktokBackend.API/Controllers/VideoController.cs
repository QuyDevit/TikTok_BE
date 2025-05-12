
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiktokBackend.API.Filters;
using TiktokBackend.Application.Commands.Videos;
using TiktokBackend.Application.Payloads;

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

        [HttpPost("upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
        public async Task<IActionResult> UploadVideo([FromForm] List <IFormFile> files, [FromForm] string description, [FromForm] string viewable, [FromForm] string[] allows)
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
            PostVideoRequest videoRequest = new PostVideoRequest
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
