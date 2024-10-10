using System.Diagnostics.CodeAnalysis;

namespace Application.Classes;

public class Result
{
    // Constructor for internal use only
    protected internal Result(bool isSuccess, Error error)
    {
        // Check for invalid combinations of isSuccess and error
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    // Indicates if the operation was successful
    public bool IsSuccess { get; }

    // Indicates if the operation failed
    public bool IsFailure => !IsSuccess;

    // The error associated with a failure
    public Error Error { get; }

    // Factory method for creating a successful result without data
    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    // Factory method for creating a failure result with an error
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    // Factory method for creating a successful result with data
    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    // Factory method for creating a failure result with an error and no data
    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }

    // Factory method for creating a result based on whether the value is null or not
    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    // Constructor for internal use only
    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    // Gets the value if the operation was successful
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value)
    {
        return Create(value);
    }
}