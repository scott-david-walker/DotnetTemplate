namespace Api.Framework;

internal interface IAuthoriser;
internal interface IAuthoriser<in T> : IAuthoriser
{
    public Task<bool> Authorise(T request);
}