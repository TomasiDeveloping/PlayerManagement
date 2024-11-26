using Application.DataTransferObjects.VsDuel;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class VsDuelProfile : Profile
{
    public VsDuelProfile()
    {
        CreateMap<VsDuel, VsDuelDto>();

        CreateMap<VsDuel, VsDuelDetailDto>();

        CreateMap<UpdateVsDuelDto, VsDuel>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));

        CreateMap<CreateVsDuelDto, VsDuel>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.EventDate, opt => opt.MapFrom(src => DateTime.Parse(src.EventDate)));
    }
}