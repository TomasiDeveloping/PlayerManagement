using Application.DataTransferObjects.DesertStorm;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class DesertStormProfile : Profile
{
    public DesertStormProfile()
    {
        CreateMap<DesertStorm, DesertStormDto>()
            .ForMember(des => des.Participants,
                opt => opt.MapFrom(src => src.DesertStormParticipants.Count(p => p.Participated)));

        CreateMap<DesertStorm, DesertStormDetailDto>()
            .ForMember(des => des.DesertStormParticipants, opt => opt.MapFrom(src => src.DesertStormParticipants))
            .ForMember(des => des.Participants,
                opt => opt.MapFrom(src => src.DesertStormParticipants.Count(p => p.Participated)));

        CreateMap<UpdateDesertStormDto, DesertStorm>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreateDesertStormDto, DesertStorm>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));
    }
}