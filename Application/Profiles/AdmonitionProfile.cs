using Application.DataTransferObjects.Admonition;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class AdmonitionProfile : Profile
{
    public AdmonitionProfile()
    {
        CreateMap<Admonition, AdmonitionDto>();

        CreateMap<CreateAdmonitionDto, Admonition>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdateAdmonitionDto, Admonition>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));
    }
}