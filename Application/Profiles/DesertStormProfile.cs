using Application.DataTransferObjects.DesertStorm;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class DesertStormProfile : Profile
{
    public DesertStormProfile()
    {
        CreateMap<DesertStorm, DesertStormDto>();

        CreateMap<UpdateDesertStormDto, DesertStorm>();

        CreateMap<CreateDesertStormDto, DesertStorm>()
            .ForMember(des => des.Year, opt => opt.MapFrom(src => DateTime.Now.Year))
            .ForMember(des => des.CalendarWeek, opt => opt.MapFrom(src => DateTime.Now.DayOfWeek));
    }
}