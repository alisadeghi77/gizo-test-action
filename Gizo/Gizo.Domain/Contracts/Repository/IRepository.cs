﻿using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Contracts.Repository;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> Query();

    IQueryBuilder<TEntity> Get();
    TEntity? GetById(object id);
    Task<TEntity?> GetByIdAsync(object id);

    void Insert(TEntity entity);
    void Insert(IEnumerable<TEntity> entityList);
    Task InsertAsync(TEntity entity, CancellationToken token = default);
    Task InsertAsync(IEnumerable<TEntity> entityList, CancellationToken token = default);

    IUpdateQueryBuilder<TEntity> Update(TEntity entityToUpdate);
    void Update<TViewModel>(TEntity entityToUpdate);
    void Update(IEnumerable<TEntity> entities);

    void Delete(object id);
    void Delete(TEntity entityToDelete);
    Task DeleteAsync(object id);

    IRepository<TEntity> EnableTracking();
    IRepository<TEntity> DisableTracking();
}
