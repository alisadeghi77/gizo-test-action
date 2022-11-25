using Gizo.Domain.Contracts.Repository;
using System.Linq.Expressions;

namespace DK.Data.EF.Repository
{
    public class ExpandSubRelation<TModel, TMember> : ExpandRelation<TModel>, IExpandSubRelation<TModel, TMember>
    {
        private readonly string _currentMember;

        public ExpandSubRelation(HashSet<string> membersToExpand, string currentMember) :base(membersToExpand)
        {
            _currentMember = currentMember;
        }
        public IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, ICollection<TRelation>>> expand)
        {
            return AddToMemberList<TRelation>(expand.Body);
        }


        public IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, IEnumerable<TRelation>>> expand)
        {
            return AddToMemberList<TRelation>(expand.Body);
        }


        public IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, List<TRelation>>> expand)
        {
            return AddToMemberList<TRelation>(expand.Body);
        }

        public IExpandSubRelation<TModel, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, TRelation>> expand)
        {
            return AddToMemberList<TRelation>(expand.Body);
        }


        private IExpandSubRelation<TModel, TRelation> AddToMemberList<TRelation>(Expression body)
        {
            var memberToExpand = body.ToString().Split('.').Skip(1).FirstOrDefault();
            var resultMember = $"{_currentMember}.{memberToExpand}";
            MembersToExpand.Remove(_currentMember);
            MembersToExpand.Add(resultMember);

            return new ExpandSubRelation<TModel, TRelation>(MembersToExpand, resultMember);
        }
    }
}