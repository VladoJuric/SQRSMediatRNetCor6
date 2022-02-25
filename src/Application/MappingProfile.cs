using Application.Administrator.RequestDtos;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Disabled automatic name mapping
            //DefaultMemberConfig.MemberMappers.Clear();
            //DefaultMemberConfig.NameMapper.NamedMappers.Clear();

            CreateMap<UserDto, User>()
                .ReverseMap()
                ;

            CreateMap<RequestUserDto, User>()
                .ReverseMap()
                ;
        }
    }
}
