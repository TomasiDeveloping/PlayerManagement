using Application.DataTransferObjects.Admonition;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class AdmonitionProfile : Profile
{
    public AdmonitionProfile()
    {
        CreateMap<Admonition, AdmonitionDto>();

        CreateMap<CreateAdmonitionDto, Admonition>();

        CreateMap<UpdateAdmonitionDto, Admonition>();
    }
}