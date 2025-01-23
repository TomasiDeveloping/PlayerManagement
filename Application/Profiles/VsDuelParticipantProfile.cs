using Application.DataTransferObjects.VsDuelParticipant;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class VsDuelParticipantProfile : Profile
{
    public VsDuelParticipantProfile()
    {
        CreateMap<VsDuelParticipant, VsDuelParticipantDto>()
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.PlayerName));

        CreateMap<VsDuelParticipantDto, VsDuelParticipant>();

        CreateMap<VsDuelParticipant, VsDuelParticipantDetailDto>()
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => src.VsDuel.EventDate));
    }
}