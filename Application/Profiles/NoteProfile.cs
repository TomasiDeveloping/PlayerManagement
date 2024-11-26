using Application.DataTransferObjects.Note;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteDto>();

        CreateMap<UpdateNoteDto, Note>()
            .ForMember(des => des.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreateNoteDto, Note>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));
    }
}