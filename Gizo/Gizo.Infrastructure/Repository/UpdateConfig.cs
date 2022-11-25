using Gizo.Domain.Contracts.Repository;
using System.Linq.Expressions;

namespace Gizo.Infrastructure.Repository;

public class UpdateConfig<TEntity> : IUpdateConfig<TEntity>
{
    public UpdateConfig()
    {
        Excludes = new List<Expression<Func<TEntity, object>>>();
        OnlyIncludes = new List<Expression<Func<TEntity, object>>>();
        Relations = new List<string>();
    }

    internal List<Expression<Func<TEntity, object>>> Excludes { set; get; }
    internal List<Expression<Func<TEntity, object>>> OnlyIncludes { set; get; }
    internal List<string> Relations { private set; get; }

    public IUpdateConfig<TEntity> Exclude(params Expression<Func<TEntity, object>>[] expressions)
    {
        Excludes.AddRange(expressions);
        return this;
    }

    public IUpdateConfig<TEntity> OnlyInclude(params Expression<Func<TEntity, object>>[] expressions)
    {
        OnlyIncludes.AddRange(expressions);
        return this;
    }

    public IUpdateConfig<TEntity> UpdateRelations(params Expression<Func<TEntity, IEnumerable<object>>>[] expressions)
    {
        Relations.AddRange(expressions.Select(s => s.Body.ToString().Split('.')[1]));
        return this;
    }
}
