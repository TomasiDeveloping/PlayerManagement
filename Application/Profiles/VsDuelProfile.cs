using Application.DataTransferObjects.VsDuel;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class VsDuelProfile : Profile
{
    public VsDuelProfile()
    {
        CreateMap<VsDuel, VsDuelDto>();

        CreateMap<UpdateVsDuelDto, VsDuel>();

        CreateMap<CreateVsDuelDto, VsDuel>()
            .ForMember(des => des.Year, opt => opt.MapFrom(src => DateTime.Now.Year))
            .ForMember(des => des.CalendarWeek, opt => opt.MapFrom(src => DateTime.Now.DayOfWeek));
    }
}