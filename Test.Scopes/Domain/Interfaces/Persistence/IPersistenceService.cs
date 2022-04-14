using System.Linq.Expressions;
using Test.Scopes.Abstractions.Domain.Aggregates;

namespace Test.Scopes.Domain.Interfaces.Persistence
{
    public interface IPersistenceService<TAggregate, TId>
        where TAggregate : AggregateRoot<TId>
        where TId : struct
    {
        Task<TId> SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task<List<TAggregate>> GetAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken);

        Task<TAggregate?> GetOneAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken);
    }
}