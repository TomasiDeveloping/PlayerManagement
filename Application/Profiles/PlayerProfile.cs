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
    }
}