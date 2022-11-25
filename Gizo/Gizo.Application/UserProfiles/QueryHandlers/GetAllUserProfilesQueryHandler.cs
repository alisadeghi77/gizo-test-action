using Gizo.Application.Models;
using Gizo.Application.UserProfiles.Queries;
using Gizo.Infrastructure;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesQueryHandler
    : IRequestHandler<GetAllUserProfilesQuery, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly DataContext _ctx;
    public GetAllUserProfilesQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<UserProfile>>();
        result.Data = await _ctx.UserProfiles.ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
