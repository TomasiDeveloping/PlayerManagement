using Application.DataTransferObjects.CustomEventCategory;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class CustomEventCategoryProfile : Profile
{
    public CustomEventCategoryProfile()
    {
        CreateMap<CustomEventCategory, CustomEventCategoryDto>();

        CreateMap<CreateCustomEventCategoryDto, CustomEventCategory>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()));

        CreateMap<UpdateCustomEventCategoryDto, CustomEventCategory>();
    }
}