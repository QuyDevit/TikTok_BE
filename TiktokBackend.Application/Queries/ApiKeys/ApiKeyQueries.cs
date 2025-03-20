using MediatR;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.ApiKeys
{
    public record ValidateApiKeyQuery(string Key) : IRequest<bool>;
    public class ValidateApiKeyQueryHandler(IApiKeyRepository apiKeyRepository) : IRequestHandler<ValidateApiKeyQuery, bool>
    {
        public async Task<bool> Handle(ValidateApiKeyQuery request, CancellationToken cancellationToken)
        {
            var result = await apiKeyRepository.GetByKeyAsync(request.Key);

            return result;
        }
    }
}
