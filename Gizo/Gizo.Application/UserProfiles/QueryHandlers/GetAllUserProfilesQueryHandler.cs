using Gizo.Application.Models;
using Gizo.Application.UserProfiles.Queries;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesQueryHandler
    : IRequestHandler<GetAllUserProfilesQuery, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly IRepository<UserProfile> _userRepository;

    public GetAllUserProfilesQueryHandler(IRepository<UserProfile> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesQuery request,
        CancellationToken token)
    {
        var result = new OperationResult<IEnumerable<UserProfile>>
        {
            Data = await _userRepository
                .Get()
                .ToListAsync()
        };

        return result;
    }
}
