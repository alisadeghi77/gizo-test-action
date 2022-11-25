using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface IExpandRelation<TModel>
{
    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, ICollection<TRelation>>> expand);
    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, IEnumerable<TRelation>>> expand);
    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, List<TRelation>>> expand);
    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, TRelation>> expand);
}
