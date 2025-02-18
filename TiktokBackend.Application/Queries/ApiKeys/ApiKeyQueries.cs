using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Application.Common;
using TiktokBackend.Domain.Entities;
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
