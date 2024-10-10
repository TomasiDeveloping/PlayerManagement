using Application.DataTransferObjects.Note;
using AutoMapper;
using Database.Entities;

namespace Application.Profiles;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteDto>();

        CreateMap<UpdateNoteDto, Note>();

        CreateMap<CreateNoteDto, Note>();
    }
}