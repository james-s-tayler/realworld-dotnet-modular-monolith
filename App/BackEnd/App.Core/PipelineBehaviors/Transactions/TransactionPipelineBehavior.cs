using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Modules;
using MediatR;

namespace App.Core.PipelineBehaviors.Transactions
{
    public class TransactionPipelineBehavior<TRequest, TResponse, TModule> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest<TResponse>
        where TModule : class, IModule
    {
        private readonly DbConnection _connection;

        public TransactionPipelineBehavior(ModuleDbConnectionWrapper<TModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
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