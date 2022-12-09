using Gizo.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gizo.Application.ClientIdentity.Commands;

public class UpdateUserProfileBasicInfoCommand : IRequest<OperationResult<bool>>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

