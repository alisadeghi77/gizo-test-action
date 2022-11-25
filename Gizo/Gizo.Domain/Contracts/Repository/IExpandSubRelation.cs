using System.Linq.Expressions;

namespace Gizo.Domain.Contracts.Repository;

public interface IExpandSubRelation<TModel, TMember> : IExpandRelation<TModel>
{
    IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, ICollection<TRelation>>> expand);
    IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, IEnumerable<TRelation>>> expand);
    IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, List<TRelation>>> expand);
    IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, TRelation>> expand);
}
