using Application.DataTransferObjects.MarshalGuard;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class MarshalGuardProfile : Profile
{
    public MarshalGuardProfile()
    {
        CreateMap<MarshalGuard, MarshalGuardDto>()
            .ForMember(des => des.Participants, opt => opt.MapFrom(src => src.MarshalGuardParticipants.Count(p => p.Participated)));

        CreateMap<MarshalGuard, MarshalGuardDetailDto>()
            .ForMember(des => des.Participants, opt => opt.MapFrom(src => src.MarshalGuardParticipants.Count(p => p.Participated)))
            .ForMember(des => des.MarshalGuardParticipants, opt => opt.MapFrom(des => des.MarshalGuardParticipants));

        CreateMap<UpdateMarshalGuardDto, MarshalGuard>()
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)))
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreateMarshalGuardDto, MarshalGuard>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));
    }
}