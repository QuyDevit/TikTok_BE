using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Application.Common;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetUsersByNameQuery(string Query,string Type ="less",int PageNumber = 1, int PageSize = 5) : IRequest<ServiceResponse<PagedResponse<User>>>;
    public class GetUsersByNameQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersByNameQuery, ServiceResponse<PagedResponse<User>>> {
        public async Task<ServiceResponse<PagedResponse<User>>> Handle(GetUsersByNameQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetListUserByNameAsync(request.Query);
            var pagedUsers = users
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var response = new PagedResponse<User>(pagedUsers, users.Count, request.PageNumber, request.PageSize);
            return ServiceResponse<PagedResponse<User>>.Ok(response, "Lấy danh sách người dùng thành công!");
        }
    }
}
