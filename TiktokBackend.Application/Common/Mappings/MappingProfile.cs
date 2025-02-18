using AutoMapper;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>(); 

        }
    }
}
