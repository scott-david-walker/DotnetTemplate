using Api.Framework;
using MediatR;

namespace Api.Behaviours;

internal class AuthorisationBehaviour<TRequest, TResponse>(IEnumerable<IAuthoriser<TRequest>> authorizers)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var authorizer = authorizers.FirstOrDefault();
        if (authorizer is not null)
        {
            var authorized = await authorizer.Authorise(request);
            if (!authorized)
            {
                throw new UnauthorizedAccessException();
            }
        }

        return await next();
    }
}