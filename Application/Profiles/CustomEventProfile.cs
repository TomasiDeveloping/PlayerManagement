﻿using Application.DataTransferObjects.CustomEvent;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class CustomEventProfile : Profile
{
    public CustomEventProfile()
    {
        CreateMap<CustomEvent, CustomEventDto>();

        CreateMap<CreateCustomEventDto, CustomEvent>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDateString)));

        CreateMap<UpdateCustomEventDto, CustomEvent>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDateString)));
    }
}