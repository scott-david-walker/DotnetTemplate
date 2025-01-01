namespace Core;

public interface ICurrentUser
{
    Task<string> Id();
}