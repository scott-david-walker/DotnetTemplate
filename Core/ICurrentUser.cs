namespace Core;

public interface ICurrentUser
{
    string Id { get; }
    string Email { get; }
}