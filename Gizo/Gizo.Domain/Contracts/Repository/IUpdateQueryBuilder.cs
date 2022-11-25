using Gizo.Domain.Contracts.Base;
using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface IUpdateQueryBuilder<TEntity>
{
    IUpdateQueryBuilder<TEntity> ExcludeCollection(Expression<Func<TEntity, IEnumerable<IEntity>>> expression);
    IUpdateQueryBuilder<TEntity> ExcludeRelation(Expression<Func<TEntity, IEntity>> expression);
    IUpdateQueryBuilder<TEntity> Exclude<TProp>(Expression<Func<TEntity, TProp>> expression);

    IUpdateQueryBuilder<TEntity> OnlyInclude<TProp>(Expression<Func<TEntity, TProp>> expression);
    IUpdateQueryBuilder<TEntity> OnlyIncludeItems(params Expression<Func<TEntity, object>>[] expressions);

    IUpdateQueryBuilder<TEntity> UpdateRelations(Expression<Func<TEntity, IEnumerable<IEntity>>> expression);
}
