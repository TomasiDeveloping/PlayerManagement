using Application.DataTransferObjects.SquadType;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class SquadTypeProfile : Profile
{
    public SquadTypeProfile()
    {
        CreateMap<SquadType, SquadTypeDto>();
    }
}