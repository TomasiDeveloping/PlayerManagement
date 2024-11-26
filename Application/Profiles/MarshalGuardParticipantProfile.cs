using Application.DataTransferObjects.MarshalGuardParticipant;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class MarshalGuardParticipantProfile : Profile   
{
    public MarshalGuardParticipantProfile()
    {
        CreateMap<MarshalGuardParticipant, MarshalGuardParticipantDto>()
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.PlayerName));

        CreateMap<CreateMarshalGuardParticipantDto, MarshalGuardParticipant>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()));

        CreateMap<UpdateMarshalGuardParticipantDto, MarshalGuardParticipant>();
    }
}