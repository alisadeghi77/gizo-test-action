using AutoMapper;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Gizo.Infrastructure.Repository;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly DataContext _context; // TODO
    private readonly DbSet<TEntity> _dbSet;
    private readonly IMapper _mapper;
    private bool _isTrackingEnabled = false;

    public Repository(DataContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;

        //context.Set<User>().Include(x => x.UserRoles)
    }

    public IQueryBuilder<TEntity> Get()
    {
        var query = _isTrackingEnabled ? _dbSet : _dbSet.AsNoTracking();

        return new QueryBuilder<TEntity>(query, _mapper);
    }

    public TEntity? GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual IQueryable<TEntity> Query()
    {
        IQueryable<TEntity> query = _dbSet;

        return query;
    }

    public virtual void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Insert(IEnumerable<TEntity> entityList)
    {
        foreach (var entity in entityList)
        {
            Insert(entity);
        }
    }

    public async Task InsertAsync(TEntity entity, CancellationToken token = default)
    {
        await _dbSet.AddAsync(entity, token);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entityList, CancellationToken token = default)
    {
        foreach (var entity in entityList)
        {
            await InsertAsync(entity, token);
        }
    }

    public virtual void Delete(object id)
    {
        var entityToDelete = _dbSet.Find(id);
        Delete(entityToDelete!);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public async Task DeleteAsync(object id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        Delete(entityToDelete!);
    }

    public virtual IUpdateQueryBuilder<TEntity> Update(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        var entry = _context.Entry(entityToUpdate);
        entry.State = EntityState.Modified;

        return new UpdateQueryBuilder<TEntity>(entityToUpdate, entry);
    }

    public virtual void Update<TViewModel>(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;

        var validEntityProp = GetSystemType(typeof(TEntity).GetProperties());
        var validViewModelProp = GetSystemType(typeof(TViewModel).GetProperties());
        var differentProp = validEntityProp.Except(validViewModelProp, new PropertyNameComparer());
        var excludePropList = GetNullType(differentProp, entityToUpdate).Select(i => i.Name);

        foreach (var excColumn in excludePropList)
            _context.Entry(entityToUpdate).Property(excColumn).IsModified = false;
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Update(entity);
        }
    }

    public IRepository<TEntity> EnableTracking()
    {
        _isTrackingEnabled = true;
        return this;
    }

    public IRepository<TEntity> DisableTracking()
    {
        _isTrackingEnabled = false;
        return this;
    }

    private List<PropertyInfo> GetSystemType(IEnumerable<PropertyInfo> propertyInfoList)
    {
        List<PropertyInfo> systemProperty = new List<PropertyInfo>();
        foreach (var property in propertyInfoList)
        {
            string typeName = "";
            if (property.PropertyType.IsGenericType)
            {
                Type genericType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                if (genericType != null)
                    typeName = genericType.FullName;
                else
                    continue;
            }
            else
                typeName = property.PropertyType.FullName;

            bool isSystemType = typeName.StartsWith("System", StringComparison.OrdinalIgnoreCase);

            if (isSystemType)
                systemProperty.Add(property);
        }
        return systemProperty;
    }

    private List<PropertyInfo> GetNullType(IEnumerable<PropertyInfo> propertyList, object entity)
    {
        List<PropertyInfo> nullValueProperty = new List<PropertyInfo>();
        object obj = (object)entity;
        foreach (var property in propertyList)
        {
            object propertyValue = property.GetValue(obj, null);
            var propertyType = property.PropertyType;
            var defaultValue = GetDefaultValue(propertyType);

            if (propertyValue == null || propertyValue.Equals(defaultValue))
                nullValueProperty.Add(property);
        }
        return nullValueProperty;
    }

    private object GetDefaultValue(Type type)
    {
        if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            return Activator.CreateInstance(type);

        return null;
    }
}

internal class PropertyNameComparer : IEqualityComparer<PropertyInfo>
{
    public bool Equals(PropertyInfo? left, PropertyInfo? right)
    {
        if (string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }

    public int GetHashCode(PropertyInfo obj)
    {
        return obj.Name.GetHashCode();
    }
}
