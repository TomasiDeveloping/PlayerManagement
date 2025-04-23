using Application.DataTransferObjects.CustomEvent;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class CustomEventProfile : Profile
{
    public CustomEventProfile()
    {
        CreateMap<CustomEvent, CustomEventDto>()
            .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.CustomEventCategory!.Name));

        CreateMap<CustomEvent, CustomEventDetailDto>()
            .ForMember(des => des.CustomEventParticipants, opt => opt.MapFrom(src => src.CustomEventParticipants));

        CreateMap<CreateCustomEventDto, CustomEvent>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));

        CreateMap<UpdateCustomEventDto, CustomEvent>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));
    }
}