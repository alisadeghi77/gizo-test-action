using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<OperationResult<UserProfile>>
{
    public long UserProfileId { get; set; }
}
