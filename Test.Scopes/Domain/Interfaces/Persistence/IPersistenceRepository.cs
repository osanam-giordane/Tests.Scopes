using System.Linq.Expressions;
using Test.Scopes.Abstractions.Domain.Aggregates;

namespace Test.Scopes.Domain.Interfaces.Persistence
{
    public interface IPersistenceRepository<TAggregate, TId>
        where TAggregate : AggregateRoot<TId>
        where TId : struct
    {
        Task<TId> AddAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task<List<TAggregate>> RecoverAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken);

        Task<TAggregate?> RecoverOneAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken);
    }
}
