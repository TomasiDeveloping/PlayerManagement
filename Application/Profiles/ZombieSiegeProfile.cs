using Application.DataTransferObjects.ZombieSiege;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class ZombieSiegeProfile : Profile
{
    public ZombieSiegeProfile()
    {
        CreateMap<ZombieSiege, ZombieSiegeDto>()
            .ForMember(des => des.TotalWavesSurvived, 
                opt => opt.MapFrom(scr => scr.ZombieSiegeParticipants.Sum(p => p.SurvivedWaves)))
            .ForMember(des => des.TotalLevel20Players,
                opt => opt.MapFrom(src => src.ZombieSiegeParticipants.Count(p => p.SurvivedWaves == 20)));

        CreateMap<ZombieSiege, ZombieSiegeDetailDto>()
            .ForMember(des => des.TotalWavesSurvived,
                opt => opt.MapFrom(scr => scr.ZombieSiegeParticipants.Sum(p => p.SurvivedWaves)))
            .ForMember(des => des.TotalLevel20Players,
                opt => opt.MapFrom(src => src.ZombieSiegeParticipants.Count(p => p.SurvivedWaves == 20)));

        CreateMap<CreateZombieSiegeDto, ZombieSiege>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));

        CreateMap<UpdateZombieSiegeDto, ZombieSiege>()
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)))
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));
    }
}