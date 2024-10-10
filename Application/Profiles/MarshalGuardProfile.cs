using Application.DataTransferObjects.MarshalGuard;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class MarshalGuardProfile : Profile
{
    public MarshalGuardProfile()
    {
        CreateMap<MarshalGuard, MarshalGuardDto>();

        CreateMap<UpdateMarshalGuardDto, MarshalGuard>();

        CreateMap<CreateMarshalGuardDto, MarshalGuard>()
            .ForMember(des => des.Year, opt => opt.MapFrom(src => DateTime.Now.Year))
            .ForMember(des => des.Month, opt => opt.MapFrom(src => DateTime.Now.Month))
            .ForMember(des => des.Day, opt => opt.MapFrom(src => DateTime.Now.Day));
    }
}