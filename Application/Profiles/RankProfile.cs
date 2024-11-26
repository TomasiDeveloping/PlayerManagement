using Application.DataTransferObjects.Rank;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class RankProfile : Profile
{
    public RankProfile()
    {
        CreateMap<Rank, RankDto>();
    }
}