using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface IUpdateConfig<TEntity>
{
    IUpdateConfig<TEntity> Exclude(params Expression<Func<TEntity, object>>[] expressions);
    IUpdateConfig<TEntity> OnlyInclude(params Expression<Func<TEntity, object>>[] expressions);
    IUpdateConfig<TEntity> UpdateRelations(params Expression<Func<TEntity, IEnumerable<object>>>[] expressions);
}
