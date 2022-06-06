using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;

namespace Conduit.Core.PipelineBehaviors.Transactions
{
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest<TResponse>
    {
        private readonly DbConnection _connection;

        public TransactionPipelineBehavior(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var transaction = await _connection.BeginTransactionAsync(cancellationToken);
            TResponse response;
            try
            {
                response = await next();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            return response;
        }
    }
}