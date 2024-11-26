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

        CreateMap<UpdateAllianceDto, Alliance>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<SignUpRequestDto, Alliance>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.CreatedOn, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.Name, opt => opt.MapFrom(src => src.AllianceName))
            .ForMember(des => des.Abbreviation, opt => opt.MapFrom(src => src.AllianceAbbreviation))
            .ForMember(des => des.Server, opt => opt.MapFrom(src => src.AllianceServer));
    }
}