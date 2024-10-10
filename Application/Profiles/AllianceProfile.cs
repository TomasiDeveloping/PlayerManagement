using Application.DataTransferObjects.Alliance;
using Application.DataTransferObjects.Authentication;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class AllianceProfile : Profile
{
    public AllianceProfile()
    {
        CreateMap<Alliance, AllianceDto>();

        CreateMap<CreateAllianceDto, Alliance>();

        CreateMap<UpdateAllianceDto, Alliance>();

        CreateMap<RegisterRequestDto, Alliance>()
            .ForMember(des => des.Name, opt => opt.MapFrom(src => src.AllianceName))
            .ForMember(des => des.Abbreviation, opt => opt.MapFrom(src => src.AllianceAbbreviation))
            .ForMember(des => des.Server, opt => opt.MapFrom(src => src.AllianceServer));
    }
}