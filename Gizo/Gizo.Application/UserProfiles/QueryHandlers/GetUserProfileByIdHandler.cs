using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Application.UserProfiles.Queries;
using MediatR;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Domain.Contracts.Repository;

namespace Gizo.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdHandler
    : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly IRepository<UserProfile> _userRepository;

    public GetUserProfileByIdHandler(IRepository<UserProfile> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken token)
    {
        var result = new OperationResult<UserProfile>();

        var profile = await _userRepository
            .Get()
            .Filter(_ => _.Id == request.UserProfileId)
            .FirstAsync();

        if (profile is null)
        {
            result.AddError(ErrorCode.NotFound,
                string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
            return result;
        }

        result.Data = profile;
        return result;
    }
}
