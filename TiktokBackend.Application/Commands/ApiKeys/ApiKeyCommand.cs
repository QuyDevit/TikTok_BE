using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.ApiKeys
{
    public record AddApiKeyCommand(Guid UserId) : IRequest<ServiceResponse<ApiKey>>;
    public class AddApiKeyCommandHandler(IApiKeyRepository apiKeyRepository) : IRequestHandler<AddApiKeyCommand, ServiceResponse<ApiKey>>
    {
        public async Task<ServiceResponse<ApiKey>> Handle(AddApiKeyCommand request, CancellationToken cancellationToken)
        {
            var newApikey = await apiKeyRepository.AddAsync(request.UserId);
            if (newApikey is null)
            {
                return ServiceResponse<ApiKey>.Fail("Api Key đang được sử dụng!");
            }
            return ServiceResponse<ApiKey>.Ok(newApikey,"Tạo Api Key thành công!");
        }
    }
}
