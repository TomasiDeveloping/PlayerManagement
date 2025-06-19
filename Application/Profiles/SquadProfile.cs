using Application.DataTransferObjects.Squad;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class SquadProfile : Profile
{
    public SquadProfile()
    {
        CreateMap<Squad, SquadDto>()
            .ForMember(des => des.TypeName, opt => opt.MapFrom(src => src.SquadType.TypeName));

        CreateMap<CreateSquadDto, Squad>()
            .ForMember(dest => dest.LastUpdateAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()));

        CreateMap<UpdateSquadDto, Squad>()
            .ForMember(des => des.LastUpdateAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}