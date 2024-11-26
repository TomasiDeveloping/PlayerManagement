using Application.DataTransferObjects.DesertStormParticipants;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class DesertStormParticipantProfile : Profile
{
    public DesertStormParticipantProfile()
    {
        CreateMap<DesertStormParticipant, DesertStormParticipantDto>()
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.PlayerName));

        CreateMap<CreateDesertStormParticipantDto, DesertStormParticipant>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()));

        CreateMap<UpdateDesertStormParticipantDto, DesertStormParticipant>();
    }
}