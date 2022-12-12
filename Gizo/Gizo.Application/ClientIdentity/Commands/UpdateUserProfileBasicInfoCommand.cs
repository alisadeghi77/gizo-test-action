using Gizo.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gizo.Application.ClientIdentity.Commands;

public sealed record UpdateUserProfileBasicInfoCommand(
    long Id,
    string FirstName,
    string LastName,
    string Email) : IRequest<OperationResult<bool>>
{ }