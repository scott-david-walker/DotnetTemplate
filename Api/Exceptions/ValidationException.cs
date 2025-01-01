using FluentValidation.Results;

namespace Api.Exceptions;

[Serializable]
public class ValidationException() : Exception("One or more validation failures have occurred.")
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Errors?.Add(propertyName, propertyFailures);
        }
    }
        
    public IDictionary<string, string[]>? Errors { get; } = new Dictionary<string, string[]>();
}