﻿using Application.DataTransferObjects.CustomEventParticipant;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class CustomEventParticipantProfile : Profile    
{
    public CustomEventParticipantProfile()
    {
        CreateMap<CustomEventParticipant, CustomEventParticipantDto>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.PlayerName));
    }
}