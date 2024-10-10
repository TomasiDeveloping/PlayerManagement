using Application.DataTransferObjects.Player;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class PlayerProfile : Profile
{
    public PlayerProfile()
    {
        CreateMap<Player, PlayerDto>()
            .ForMember(des => des.RankName, opt => opt.MapFrom(src => src.Rank.Name));

        CreateMap<CreatePlayerDto, Player>();

        CreateMap<UpdatePlayerDto, Player>();
    }
}