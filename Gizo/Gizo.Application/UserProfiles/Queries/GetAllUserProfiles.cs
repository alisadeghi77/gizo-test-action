using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Application.Models;
using MediatR;


namespace Gizo.Application.UserProfiles.Queries;

public class GetAllUserProfiles : IRequest<OperationResult<IEnumerable<UserProfile>>>
{
}
