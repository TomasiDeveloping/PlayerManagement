using Application.DataTransferObjects.Player;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class PlayerProfile : Profile
{
    public PlayerProfile()
    {
        CreateMap<Player, PlayerDto>()
            .ForMember(des => des.NotesCount, opt => opt.MapFrom(src => src.Notes.Count))
            .ForMember(des => des.AdmonitionsCount, opt => opt.MapFrom(src => src.Admonitions.Count))
            .ForMember(des => des.RankName, opt => opt.MapFrom(src => src.Rank.Name));

        CreateMap<CreatePlayerDto, Player>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.IsDismissed, opt => opt.MapFrom(src => false))
            .ForMember(des => des.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdatePlayerDto, Player>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<Player, DismissPlayerInformationDto>()
            .ForMember(des => des.VsDuelParticipants,
                opt => opt.MapFrom(src =>
                    src.VsDuelParticipants
                        .OrderByDescending(x => x.VsDuel.EventDate)
                        .Take(3)
                        .Select(x => new DismissVsDuelParticipant(x.VsDuel.EventDate, x.WeeklyPoints))))
            .ForMember(des => des.MarshalGuardParticipants,
                opt => opt.MapFrom(src => 
                    src.MarshalGuardParticipants
                        .OrderByDescending(x => x.MarshalGuard.EventDate)
                        .Take(3)
                        .Select(x => new DismissMarshalParticipant(x.MarshalGuard.EventDate, x.Participated))))
                .ForMember(des => des.DesertStormParticipants,
                    opt => opt.MapFrom(src =>
                        src.DesertStormParticipants
                            .OrderByDescending(x => x.DesertStorm.EventDate)
                            .Take(3)
                            .Select(x => new DismissDesertStormParticipant(x.DesertStorm.EventDate, x.Participated))))
            .ForMember(des => des.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(des => des.Admonitions, opt => opt.MapFrom(src => src.Admonitions));
    }
}