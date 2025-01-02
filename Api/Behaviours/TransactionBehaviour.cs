using MediatR;
using Persistence;

namespace Api.Behaviours;

public class TransactionBehaviour<TRequest, TResponse>(
    ApplicationDbContext context,
    ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();

            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch (Exception)
        {                
            logger.LogError("Rolling back transaction {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}