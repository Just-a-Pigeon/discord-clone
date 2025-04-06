using FluentValidation.Results;

namespace DiscordClone.Domain;

public record ValidationError(string ErrorCode, string Reason, string Name)
{
    public static ValidationError InvalidInput(string reason, string name) =>
        new ValidationError("InvalidInput", reason, name);
    
    public static ValidationError FileError(string reason, string name) =>
        new ValidationError("FileError", reason, name);
    
    public static ValidationError NotFound(string reason, string name) =>
        new ValidationError("NotFound", reason, name);
}

public static class ValidationErrorExtensions
{
    public static ValidationFailure ToValidationFailure(this ValidationError validationError, object attemptedValue) => new ValidationFailure(validationError.Name, validationError.Reason, attemptedValue);
}