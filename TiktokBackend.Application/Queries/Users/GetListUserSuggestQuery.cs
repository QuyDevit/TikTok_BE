using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetListUserSuggestQuery(int Page,int Per_page):IRequest<PagedResponse<UserDto>>;
    public class GetListUserSuggestQueryHandler : IRequestHandler<GetListUserSuggestQuery, PagedResponse<UserDto>>
    {
        private readonly IUserSearchService _userSearchService;

        public GetListUserSuggestQueryHandler(IUserSearchService userSearchService)
        {
           _userSearchService = userSearchService;
        }

        public async Task<PagedResponse<UserDto>> Handle(GetListUserSuggestQuery request, CancellationToken cancellationToken)
        {
            var (users, totalRecords) = await _userSearchService.GetListUserSuggestAsync(request.Page, request.Per_page);

            return PagedResponse<UserDto>.Create(users, request.Page, request.Per_page,(int)totalRecords);
        }
    }
}
