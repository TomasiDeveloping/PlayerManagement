public record Error(string Code, string Name)
{
    /// <summary>
    ///     Represents a common error for null values.
    /// </summary>
    public static Error NullValue = new("Error.NullValue", "Null value was provided");

    /// <summary>
    ///     Represents a default error with an empty code and name.
    /// </summary>
    public static Error None => new(string.Empty, string.Empty);
}