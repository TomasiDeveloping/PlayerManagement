using Application.DataTransferObjects.Authentication;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<RegisterRequestDto, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(des => des.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));
    }
}