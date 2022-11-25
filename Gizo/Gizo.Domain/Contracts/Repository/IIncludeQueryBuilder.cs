using Gizo.Domain.Contracts.Base;
using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface IIncludeQueryBuilder<TEntity, TMember> : IQueryBuilder<TEntity>
    where TEntity : class, IEntity
{
    IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(
        Expression<Func<TMember, IEnumerable<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(
        Expression<Func<TMember, ICollection<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(
        Expression<Func<TMember, List<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(
        Expression<Func<TMember, TRelation>> member);
}
