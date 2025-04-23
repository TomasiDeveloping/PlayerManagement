namespace Application.Errors;

public class CustomEventCategoryErrors
{
    public static readonly Error NotFound = new("Error.CustomEventCategory.NotFound",
        "The custom event category with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.CustomEventCategory.IdConflict", "There is a conflict with the id's");
}