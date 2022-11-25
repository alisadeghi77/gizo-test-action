using AutoMapper;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Gizo.Infrastructure.Repository;

public class IncludeQueryBuilder<TEntity, TMember> : QueryBuilder<TEntity>, IIncludeQueryBuilder<TEntity, TMember>
    where TEntity : class, IEntity
{
    private readonly IIncludableQueryable<TEntity, TMember> _singleMember;
    private readonly IIncludableQueryable<TEntity, IEnumerable<TMember>> _collectionMember;
    private readonly IMapper _mapper;

    public IncludeQueryBuilder(IIncludableQueryable<TEntity, TMember> query, IMapper mapper) : base(query, mapper)
    {
        _singleMember = query;
        _mapper = mapper;
    }

    public IncludeQueryBuilder(IIncludableQueryable<TEntity, IEnumerable<TMember>> query, IMapper mapper) : base(query, mapper)
    {
        _collectionMember = query;
        _mapper = mapper;
    }

    public IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, IEnumerable<TRelation>>> member)
    {
        var query = _singleMember != null
            ? _singleMember.ThenInclude(member)
            : _collectionMember.ThenInclude(member);

        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, ICollection<TRelation>>> member)
    {
        var query = _singleMember != null 
            ? _singleMember.ThenInclude(member)
            : _collectionMember.ThenInclude(member);

        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, List<TRelation>>> member)
    {
        var query = _singleMember != null 
            ? _singleMember.ThenInclude(member)
            : _collectionMember.ThenInclude(member);

        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }

    public IIncludeQueryBuilder<TEntity, TRelation> ThenInclude<TRelation>(Expression<Func<TMember, TRelation>> member)
    {
        var query = _singleMember != null 
            ? _singleMember.ThenInclude(member)
            : _collectionMember.ThenInclude(member);

        return new IncludeQueryBuilder<TEntity, TRelation>(query, _mapper);
    }
}
