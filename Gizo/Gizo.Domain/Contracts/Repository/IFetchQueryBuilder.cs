using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Contracts.Repository;

public interface IFetchQueryBuilder<TEntity>
{
    IQueryable<TEntity> AsQueryable();

    List<TEntity> ToList();
    Task<List<TEntity>> ToListAsync();

    TEntity? First();
    Task<TEntity?> FirstAsync();
    
    PagingResult<TEntity> ToPaging();
    Task<PagingResult<TEntity>> ToPagingAsync();
}
