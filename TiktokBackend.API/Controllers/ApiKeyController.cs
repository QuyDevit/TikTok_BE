using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiktokBackend.Application.Commands.ApiKeys;

namespace TiktokBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly ISender _sender;
        public ApiKeyController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddApiKeyAsync([FromBody] Guid userId)
        {
            var result = await _sender.Send(new AddApiKeyCommand(userId));
            return Ok(result);
        }
    }
}
