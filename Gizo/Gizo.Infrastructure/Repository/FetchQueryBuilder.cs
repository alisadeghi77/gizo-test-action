using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Infrastructure.Repository;

public class FetchQueryBuilder<TEntity> : IFetchQueryBuilder<TEntity>
{
    protected IQueryable<TEntity> Query;

    protected PagingConfig PagingConfig;

    public FetchQueryBuilder(IQueryable<TEntity> query, PagingConfig pagingConfig)
    {
        Query = query;
        PagingConfig = pagingConfig;
    }

    public IQueryable<TEntity> AsQueryable()
    {
        HandlePaging();
        return Query;
    }

    public List<TEntity> ToList()
    {
        HandlePaging();
        return Query.ToList();
    }

    public async Task<List<TEntity>> ToListAsync(CancellationToken token = default)
    {
        HandlePaging();
        return await Query.ToListAsync(token);
    }

    public TEntity? First()
    {
        HandlePaging();
        return Query.FirstOrDefault();
    }

    public async Task<TEntity?> FirstAsync(CancellationToken token = default)
    {
        HandlePaging();
        return await Query.FirstOrDefaultAsync(token);
    }

    public PagingResult<TEntity> ToPaging()
    {
        var result = new PagingResult<TEntity>
        {
            Count = Query.Count()
        };

        HandlePaging();

        result.Data = Query.ToList();
        return result;
    }

    public async Task<PagingResult<TEntity>> ToPagingAsync(CancellationToken token = default)
    {
        var result = new PagingResult<TEntity>
        {
            Count = await Query.CountAsync(token)
        };

        HandlePaging();

        result.Data = await Query.ToListAsync(token);
        return result;
    }

    private void HandlePaging()
    {
        if (PagingConfig != null)
            Query = Query.Skip(PagingConfig.Skip).Take(PagingConfig.Take);
    }
}
