using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Repository;
using Gizo.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System.Linq.Expressions;

namespace Gizo.Infrastructure.Repository;

public class UpdateQueryBuilder<TEntity> : IUpdateQueryBuilder<TEntity>
    where TEntity : class
{
    private readonly TEntity _entity;
    private readonly EntityEntry<TEntity> _entityEntry;
    private readonly List<string> _onlyIncludedProps;

    public UpdateQueryBuilder(TEntity entity, EntityEntry<TEntity> entityEntry)
    {
        _entity = entity;
        _entityEntry = entityEntry;
        _onlyIncludedProps = new List<string>();
    }

    public IUpdateQueryBuilder<TEntity> ExcludeCollection(Expression<Func<TEntity, IEnumerable<IEntity>>> expression)
    {
        _entityEntry.Collection(expression).IsModified = false;
        foreach (var item in _entityEntry.Collection(expression).CurrentValue)
        {
            _entityEntry.Context.Entry(item).State = EntityState.Detached;
        }

        return this;
    }

    public IUpdateQueryBuilder<TEntity> ExcludeRelation(Expression<Func<TEntity, IEntity>> expression)
    {
        _entityEntry.Reference(expression).IsModified = false;
        _entityEntry.Reference(expression).TargetEntry.State = EntityState.Detached;
        return this;
    }

    public IUpdateQueryBuilder<TEntity> Exclude<TProp>(Expression<Func<TEntity, TProp>> expression)
    {
        _entityEntry.Property(expression).IsModified = false;
        return this;
    }

    public IUpdateQueryBuilder<TEntity> OnlyInclude<TProp>(Expression<Func<TEntity, TProp>> expression)
    {
        var propertyToInclude = _entityEntry.Property(expression);

        //exclude rest properties except for current property and properties that included before
        foreach (var excludeProp in _entityEntry.Properties.Where(w =>
            w != propertyToInclude && !_onlyIncludedProps.Contains(w.Metadata.Name)))
        {
            _onlyIncludedProps.Add(propertyToInclude.Metadata.Name);
            excludeProp.IsModified = false;
        }

        propertyToInclude.IsModified = true;

        return this;
    }

    public IUpdateQueryBuilder<TEntity> OnlyIncludeItems(params Expression<Func<TEntity, object>>[] expressions)
    {
        foreach (var excludeProp in _entityEntry.Properties.Where(w => !expressions.Any(x => _entityEntry.Property(x).Metadata.Name == w.Metadata.Name)))
        {
            excludeProp.IsModified = false;
        }

        return this;
    }

    public IUpdateQueryBuilder<TEntity> UpdateRelations(Expression<Func<TEntity, IEnumerable<IEntity>>> expression)
    {
        var newData = _entityEntry.Collection(expression).CurrentValue;

        var newDataEntryList = new List<EntityEntry>();

        //remove trackers:
        if (newData != null)
        {
            foreach (var i in newData)
            {
                var newEntry = _entityEntry.Context.Entry(i);
                newEntry.State = EntityState.Detached;
                newDataEntryList.Add(newEntry);
            }
        }

        _entityEntry.Collection(expression).CurrentValue = null;

        // get fresh data from db
        _entityEntry.Collection(expression).Load();
        var dbEntryList = CloneAsEntry(_entityEntry.Collection(expression).CurrentValue);
        if (dbEntryList.NotNullAny())
        {
            _entityEntry.Collection(expression).CurrentValue = null;
        }

        foreach (var dbEntry in dbEntryList)
        {
            var newEntry = newDataEntryList.FirstOrDefault(newEntry => PrimaryKeyEqual(dbEntry, newEntry));
            //data exist both in db and new Data => update!
            if (newEntry != null)
            {
                dbEntry.State = EntityState.Detached;
                newEntry.State = EntityState.Modified;
                newDataEntryList.Remove(newEntry);
            }
            //data exist in db but not in new data => delete!
            else
            {
                dbEntry.State = EntityState.Deleted;
            }
        }

        //data exist in new data but not in db => add!
        foreach (var remainingItem in newDataEntryList)
        {
            remainingItem.State = EntityState.Added;
        }

        return this;
    }


    private List<EntityEntry> CloneAsEntry(IEnumerable data)
    {
        return (from object item in data select _entityEntry.Context.Entry(item)).ToList();
    }

    private static bool PrimaryKeyEqual(EntityEntry entry1, EntityEntry entry2)
    {
        var entry1PrimaryProps = entry1.Metadata.FindPrimaryKey().Properties.Select(s => s.Name);
        var entry2PrimaryProps = entry2.Metadata.FindPrimaryKey().Properties.Select(s => s.Name);

        return entry1PrimaryProps.All(entry1Prop => entry2PrimaryProps.Any(entry2Prop => entry2Prop == entry1Prop &&
            entry1.Property(entry1Prop).CurrentValue.ToString() == entry2.Property(entry2Prop).CurrentValue.ToString()
        ));
    }
}
