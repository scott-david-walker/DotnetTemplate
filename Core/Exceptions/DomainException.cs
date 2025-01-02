namespace Core.Exceptions;

/// <summary>
/// Base exception class for any domain error.
/// Mimics the "ProblemDetails" class and allows the setting of a status code so that exceptions can bubble up and be handled by middleware
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Status Code that describes the exception
    /// </summary>
    public int StatusCode { get; protected set; } = 500;
    
    /// <summary>
    /// A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to occurrence
    /// of the problem, except for purposes of localization(e.g., using proactive content negotiation;
    /// see[RFC7231], Section 3.4).
    /// </summary>
    public string Title => Message;
    
    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    public string? Detail { get; protected set; }
    
    /// <summary>
    /// Gets the <see cref="IDictionary{TKey, TValue}"/> for extension members.
    /// <param>Problem type definitions MAY extend the problem details object with additional members. Extension members appear in the same namespace as other members of a problem type.</param>
    /// </summary>
    /// <remarks>
    /// The round-tripping behavior for <see cref="Extensions"/> is determined by the implementation of the Input \ Output formatters.
    /// In particular, complex types or collection types may not round-trip to the original type when using the built-in JSON or XML formatters.
    /// </remarks>
    public Dictionary<string, object> Extensions { get; } = new();

    /// <summary>
    /// Instantiates a new instance of a <exception cref="DomainException"></exception>
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    protected DomainException(string message) : base(message)
    {
    }
}