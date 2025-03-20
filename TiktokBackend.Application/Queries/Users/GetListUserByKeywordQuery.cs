﻿using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetListUserByKeywordQuery(string Keyword,string Type,int Page,int Loadmore):IRequest<PagedResponse<UserDto>>;
    public class GetListUserByKeywordQueryHandler : IRequestHandler<GetListUserByKeywordQuery, PagedResponse<UserDto>>
    {
        private readonly IUserSearchService _userSearchService;

        public GetListUserByKeywordQueryHandler(IUserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
        }
        public async Task<PagedResponse<UserDto>> Handle(GetListUserByKeywordQuery request, CancellationToken cancellationToken)
        {
            int pageSize = request.Type == "less" ? 5 : 10;
            int skip = (request.Page - 1) * pageSize;

            var users = await _userSearchService.SearchUsersAsync(request.Keyword);

            var pagedUsers = users.Skip(skip).Take(pageSize).ToList();
            int totalRecords = users.Count;

            return PagedResponse<UserDto>.Create(pagedUsers, request.Page, pageSize, totalRecords);
        }
    }
}
