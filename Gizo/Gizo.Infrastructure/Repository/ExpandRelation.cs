using Gizo.Domain.Contracts.Repository;
using System.Linq.Expressions;

namespace DK.Data.EF.Repository;

public class ExpandRelation<TModel> : IExpandRelation<TModel>
{
    protected HashSet<string> MembersToExpand;

    public ExpandRelation(HashSet<string> membersToExpand)
    {
        MembersToExpand = membersToExpand;
    }

    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, ICollection<TRelation>>> expand)
    {
        return AddToMemberList<TRelation>(expand.Body);
    }

    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, IEnumerable<TRelation>>> expand)
    {
        return AddToMemberList<TRelation>(expand.Body);
    }

    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, List<TRelation>>> expand)
    {
        return AddToMemberList<TRelation>(expand.Body);
    }

    public IExpandSubRelation<TModel, TRelation> Include<TRelation>(Expression<Func<TModel, TRelation>> expand)
    {
        return AddToMemberList<TRelation>(expand.Body);
    }

    private IExpandSubRelation<TModel, TRelation> AddToMemberList<TRelation>(Expression body)
    {
        var memberToExpand = body.ToString().Split('.').Skip(1).FirstOrDefault();
        MembersToExpand.Add(memberToExpand);
        return new ExpandSubRelation<TModel, TRelation>(MembersToExpand, memberToExpand);
    }
}
