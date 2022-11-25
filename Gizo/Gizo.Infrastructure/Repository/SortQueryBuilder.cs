using AutoMapper;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enumeration;
using Gizo.Domain.Contracts.Repository;
using System.Linq.Expressions;

namespace Gizo.Infrastructure.Repository;

public class SortQueryBuilder<TEntity> : QueryBuilder<TEntity>, ISortQueryBuilder<TEntity>
    where TEntity : class, IEntity
{
    private readonly IOrderedQueryable<TEntity> _orderedQuery;
    private readonly IMapper _mapper;

    public SortQueryBuilder(IOrderedQueryable<TEntity> query, IMapper mapper, PagingConfig pagingConfig) : base(query, mapper, pagingConfig)
    {
        _orderedQuery = query;
        _mapper = mapper;
    }

    public ISortQueryBuilder<TEntity> ThenSort<TKey>(Expression<Func<TEntity, TKey>> selector)
    {
        Query = _orderedQuery.ThenBy(selector);
        return this;
    }

    public ISortQueryBuilder<TEntity> ThenSortDescending<TKey>(Expression<Func<TEntity, TKey>> selector)
    {
        Query = _orderedQuery.ThenByDescending(selector);
        return this;
    }

    public ISortQueryBuilder<TEntity> ThenSort(string columnName, SortType sortType)
    {
        var orderFunc = CreateOrder(columnName, sortType, true);

        var query = orderFunc(_orderedQuery);

        return new SortQueryBuilder<TEntity>(query, _mapper, PagingConfig);
    }

    public IQueryBuilder<TEntity> Page(int page, int pageSize)
    {

        if (page != 0 && pageSize != 0)
            PagingConfig = new PagingConfig((page - 1) * pageSize, pageSize);

        return this;
    }
}
