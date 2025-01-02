namespace Core.Exceptions;


/// <summary>
/// An exception that can be thrown whenever a resource is not found
/// </summary>
public class NotFoundException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/>
    /// </summary>
    /// <param name="id">An identifier which is the identifier of the resource that was not found</param>
    public NotFoundException(string id) : base($"Identifier '{id}' not found.")
    {
        StatusCode = 404;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/>
    /// </summary>
    /// <param name="id">An identifier which is the identifier of the resource that was not found</param>
    /// <param name="type">Type of the identifier. Type name is used in the exception message</param>
    public NotFoundException(string id, Type type) : base($"{type.Name} {id} not found.")
    {
        StatusCode = 404;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/>
    /// </summary>
    /// <param name="message">A composite format string, like in string.Format()</param>
    /// <param name="args">A string array that contains one or more strings to format.</param>
    public NotFoundException(string message, params string[] args) : base(string.Format(message, args))
    {
        StatusCode = 404;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/>
    /// </summary>
    /// <typeparam name="T">Type of the object that was not found</typeparam>
    /// <param name="id">An identifier which is the identifier of the resource that was not found</param>
    public static NotFoundException ForType<T>(string id) => new(id, typeof(T));
}