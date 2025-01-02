namespace Core.Exceptions;

/// <summary>
/// An exception that can be used when something doesn't make sense from a business perspective
/// </summary>
public class LogicException : DomainException
{
    /// <summary>Initializes a new instance of the <exception cref="LogicException"></exception> class with a specified error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public LogicException(string message) : base(message)
    {
        StatusCode = 400;
    }

    /// <summary>
    /// Initializes a new instance of the <exception cref="LogicException"></exception> class with a specified error message and detailed explanation.
    /// </summary>
    /// <param name="message">The message that summarises the error</param>
    /// <param name="detail">The detailed explanation of the error</param>
    public LogicException(string message, string detail) : base(message)
    {
        StatusCode = 400;
        Detail = detail;
    }
}