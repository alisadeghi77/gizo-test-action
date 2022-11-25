using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enums;
using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface ISortQueryBuilder<TEntity> : IQueryBuilder<TEntity>
    where TEntity : class, IEntity
{
    ISortQueryBuilder<TEntity> ThenSort<TKey>(Expression<Func<TEntity, TKey>> selector);
    ISortQueryBuilder<TEntity> ThenSortDescending<TKey>(Expression<Func<TEntity, TKey>> selector);
    ISortQueryBuilder<TEntity> ThenSort(string columnName, SortType sortType);
    IQueryBuilder<TEntity> Page(int page, int pageSize);
}
