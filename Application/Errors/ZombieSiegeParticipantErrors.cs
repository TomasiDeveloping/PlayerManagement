namespace Application.Errors;

public static class ZombieSiegeParticipantErrors
{
    public static readonly Error NotFound = new("Error.ZombieSiegeParticipant.NotFound",
        "The zombie siege participant with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.ZombieSiegeParticipant.IdConflict", "There is a conflict with the id's");
}