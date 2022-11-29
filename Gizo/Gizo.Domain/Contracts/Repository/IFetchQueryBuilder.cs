using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Contracts.Repository;

public interface IFetchQueryBuilder<TEntity>
{
    IQueryable<TEntity> AsQueryable();

    List<TEntity> ToList();
    Task<List<TEntity>> ToListAsync(CancellationToken token = default);

    TEntity? First();
    Task<TEntity?> FirstAsync(CancellationToken token = default);

    PagingResult<TEntity> ToPaging();
    Task<PagingResult<TEntity>> ToPagingAsync(CancellationToken token = default);
}
