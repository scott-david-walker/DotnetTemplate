namespace Core.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string identifier) : base($"'{identifier}' already exists.")
    {
        StatusCode = 409;
    }
}