using Application.DataTransferObjects.Authentication;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<SignUpRequestDto, User>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(des => des.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));

        CreateMap<RegisterUserDto, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(des => des.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));
    }
}