using Application.DataTransferObjects.VsDuelLeague;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class VsDuelLeagueProfile : Profile
{
    public VsDuelLeagueProfile()
    {
        CreateMap<VsDuelLeague, VsDuelLeagueDto>();
    }
}