using Application.DataTransferObjects.ZombieSiegeParticipant;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class ZombieSiegeParticipantProfile : Profile
{
    public ZombieSiegeParticipantProfile()
    {
        CreateMap<ZombieSiegeParticipant, ZombieSiegeParticipantDto>()
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.PlayerName));

        CreateMap<CreateZombieSiegeParticipantDto, ZombieSiegeParticipant>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()));

        CreateMap<UpdateZombieSiegeParticipantDto, ZombieSiegeParticipant>();
    }
}