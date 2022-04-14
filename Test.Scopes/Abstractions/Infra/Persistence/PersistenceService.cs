using System.Linq.Expressions;
using Test.Scopes.Abstractions.Domain.Aggregates;
using Test.Scopes.Domain.Interfaces.Persistence;

namespace Test.Scopes.Abstractions.Infra.Persistence
{
    public abstract class PersistenceService<TAggregate, TId> : IPersistenceService<TAggregate, TId>
        where TAggregate : AggregateRoot<TId>
        where TId : struct
    {
        private readonly IPersistenceRepository<TAggregate, TId> _repository;

        public PersistenceService(IPersistenceRepository<TAggregate, TId> repository)
            => _repository = repository;

        public async Task<TId> SaveAsync(TAggregate aggregate, CancellationToken cancellationToken)
        {
            if (aggregate.IsValid is false)
                return default;

            if(aggregate.Id.Equals(default(TId)))
                return await _repository.AddAsync(aggregate, cancellationToken);

            await _repository.UpdateAsync(aggregate, cancellationToken);
            return aggregate.Id;
        }

        public Task<List<TAggregate>> GetAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken)
            => _repository.RecoverAsync(predicate, cancellationToken);

        public Task<TAggregate?> GetOneAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken)
            => _repository.RecoverOneAsync(predicate, cancellationToken);
    }
}