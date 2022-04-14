using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.Scopes.Abstractions.Domain.Aggregates;
using Test.Scopes.Domain.Interfaces.Persistence;

namespace Test.Scopes.Abstractions.Infra.Persistence
{
    public abstract class PersistenceRepository<TAggregate, TId> : IPersistenceRepository<TAggregate, TId>
        where TAggregate : AggregateRoot<TId>
        where TId : struct
    {
        private readonly DbContext _dbContext;

        public PersistenceRepository(DbContext dbContext)
            => _dbContext = dbContext;

        public async Task<TId> AddAsync(TAggregate aggregate, CancellationToken cancellationToken)
        {
            var entityEntry = await _dbContext.Set<TAggregate>().AddAsync(aggregate, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entityEntry.Entity.Id;
        }

        public async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken)
        {
            _dbContext.Attach(aggregate);
            _dbContext.Entry(aggregate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<List<TAggregate>> RecoverAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken)
            => _dbContext
                .Set<TAggregate>()
                .Where(predicate)
                .ToListAsync(cancellationToken);

        public Task<TAggregate?> RecoverOneAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken)
            => _dbContext
                .Set<TAggregate>()
                .FirstOrDefaultAsync(predicate, cancellationToken);
    }
}