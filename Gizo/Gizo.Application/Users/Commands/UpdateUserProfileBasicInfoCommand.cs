using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Users.Commands;

public class UpdateUserProfileBasicInfoCommand : IRequest<OperationResult<bool>>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}