using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enumeration;
using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface IQueryBuilder<TEntity> : IFetchQueryBuilder<TEntity>
    where TEntity : class, IEntity
{
    IQueryBuilder<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
    IQueryBuilder<TEntity> FilterIf(bool condition, Expression<Func<TEntity, bool>> filter);

    ISortQueryBuilder<TEntity> Sort<TKey>(Expression<Func<TEntity, TKey>> selector);
    ISortQueryBuilder<TEntity> SortDescending<TKey>(Expression<Func<TEntity, TKey>> selector);
    ISortQueryBuilder<TEntity> Sort(string columnName, SortType sortType);

    IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, IQueryable<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, ICollection<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, IEnumerable<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, List<TRelation>>> member);

    IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, TRelation>> member);

    int Sum(Func<TEntity, int> selector);
    long Sum(Func<TEntity, long> selector);
    decimal Sum(Func<TEntity, decimal> selector);
    Task<int> SumAsync(Expression<Func<TEntity, int>> selector);
    Task<long> SumAsync(Expression<Func<TEntity, long>> selector);
    Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector);

    bool Any();
    Task<bool> AnyAsync();

    Task<int> Count();

    IFetchQueryBuilder<TViewModel> ProjectTo<TViewModel>(Action<IExpandRelation<TViewModel>> expand = null);
    IFetchQueryBuilder<TViewModel> ProjectTo<TViewModel>(string[] expand);
    IFetchQueryBuilder<TProjection> Project<TProjection>(Expression<Func<TEntity, TProjection>> selectExpression);
}
