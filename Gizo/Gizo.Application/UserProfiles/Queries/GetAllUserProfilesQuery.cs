using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.UserProfiles.Queries;

public class GetAllUserProfilesQuery : IRequest<OperationResult<IEnumerable<UserProfile>>>
{
}
