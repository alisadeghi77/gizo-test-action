using AutoMapper;
using AutoMapper.QueryableExtensions;
using DK.Data.EF.Repository;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enums;
using Gizo.Domain.Contracts.Repository;
using Gizo.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Gizo.Infrastructure.Repository;

public class QueryBuilder<TEntity> : FetchQueryBuilder<TEntity>, IQueryBuilder<TEntity>
    where TEntity : class, IEntity
{
    private readonly IMapper _mapper;

    public QueryBuilder(IQueryable<TEntity> query, IMapper mapper) : base(query, null)
    {
        _mapper = mapper;
    }

    public QueryBuilder(IQueryable<TEntity> query, IMapper mapper, PagingConfig pagingConfig) : base(query, pagingConfig)
    {
        _mapper = mapper;
    }

    public IQueryBuilder<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
    {
        Query = Query.Where(filter);
        return this;
    }

    public ISortQueryBuilder<TEntity> Sort<TKey>(Expression<Func<TEntity, TKey>> selector)
    {
        var query = Query.OrderBy(selector);

        return new SortQueryBuilder<TEntity>(query, _mapper, PagingConfig);
    }

    public ISortQueryBuilder<TEntity> SortDescending<TKey>(Expression<Func<TEntity, TKey>> selector)
    {
        var query = Query.OrderByDescending(selector);
        return new SortQueryBuilder<TEntity>(query, _mapper, PagingConfig);
    }

    public ISortQueryBuilder<TEntity> Sort(string columnName, SortType sortType)
    {
        var orderFunc = CreateOrder(columnName, sortType);
        var query = orderFunc(Query);

        return new SortQueryBuilder<TEntity>(query, _mapper, PagingConfig);
    }

    public virtual IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, IQueryable<TRelation>>> member)
    {
        var query = Query.Include(member);
        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public virtual IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, ICollection<TRelation>>> member)
    {
        var query = Query.Include(member);
        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public virtual IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, IEnumerable<TRelation>>> member)
    {
        var query = Query.Include(member);
        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public virtual IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, List<TRelation>>> member)
    {
        var query = Query.Include(member);
        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public virtual IIncludeQueryBuilder<TEntity, TRelation> Include<TRelation>(
        Expression<Func<TEntity, TRelation>> member)
    {
        var query = Query.Include(member);
        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public int Sum(Func<TEntity, int> selector) => Query.Sum(selector);
    public long Sum(Func<TEntity, long> selector) => Query.Sum(selector);
    public decimal Sum(Func<TEntity, decimal> selector) => Query.Sum(selector);
    public async Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken token = default)
        => await Query.SumAsync(selector, token);
    public async Task<long> SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken token = default)
        => await Query.SumAsync(selector, token);
    public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken token = default)
        => await Query.SumAsync(selector, token);

    public bool Any() => Query.Any();
    public async Task<bool> AnyAsync(CancellationToken token = default) => await Query.AnyAsync(token);

    public async Task<int> CountAsync(CancellationToken token = default) => await Query.CountAsync(token);

    public IFetchQueryBuilder<TViewModel> ProjectTo<TViewModel>(Action<IExpandRelation<TViewModel>> expand = null)
    {
        var membersToExpand = new HashSet<string>();

        expand?.Invoke(new ExpandRelation<TViewModel>(membersToExpand));

        var query = Query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, membersToExpand.ToArray());
        return new FetchQueryBuilder<TViewModel>(query, PagingConfig);
    }

    public IFetchQueryBuilder<TViewModel> ProjectTo<TViewModel>(string[] expand)
    {
        var query = Query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, expand);

        return new FetchQueryBuilder<TViewModel>(query, PagingConfig);
    }

    public IFetchQueryBuilder<TProjection> Project<TProjection>(Expression<Func<TEntity, TProjection>> selectExpression)
    {
        var query = Query.Select(selectExpression);
        return new FetchQueryBuilder<TProjection>(query, PagingConfig);
    }

    protected static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> CreateOrder(
        string columnName,
        SortType sortType,
        bool isThenBy = false)
    {
        var sourceType = typeof(TEntity);
        var parameter = Expression.Parameter(sourceType, "p");
        Type destinationType = sourceType;
        LambdaExpression orderByExp = null;
        Expression relationExpression = parameter;

        var splitedByDot = columnName.Split('.');
        var columnIndex = 0;

        while (columnIndex < splitedByDot.Length)
        {
            var property = GetProperty(splitedByDot[columnIndex], destinationType);

            if (property == null)
                return x => x as IOrderedQueryable<TEntity>;

            relationExpression = Expression.Property(relationExpression, property);
            destinationType = property.PropertyType;
            columnIndex++;
        }

        orderByExp = Expression.Lambda(relationExpression, parameter);

        var orderingFunc = sortType switch
        {
            SortType.Asc when !isThenBy => "OrderBy",
            SortType.Asc when isThenBy => "ThenBy",
            SortType.Desc when !isThenBy => "OrderByDescending",
            SortType.Desc when isThenBy => "ThenByDescending",
            _ => ""
        };

        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderDelegate = p =>
        {
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                                                orderingFunc,
                                                new[] { sourceType, destinationType }, p.Expression,
                                                Expression.Quote(orderByExp));
            return p.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        };

        return orderDelegate;
    }

    private static PropertyInfo GetProperty(string propertyName, Type type)
    {
        var property = type.GetProperty(propertyName);

        if (property == null || property.PropertyType.IsEnumerable())
        {
            property = type.GetProperty(nameof(IEntity<object>.Id));
        }

        return property;
    }

    public IQueryBuilder<TEntity> FilterIf(bool condition, Expression<Func<TEntity, bool>> filter)
    {
        if (condition)
        {
            Query = Query.Where(filter);
        }

        return this;
    }
}
