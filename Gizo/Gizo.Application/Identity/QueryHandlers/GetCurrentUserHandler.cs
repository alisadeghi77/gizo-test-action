using AutoMapper;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Identity.Queries;
using Gizo.Application.Models;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Identity.QueryHandlers;

public class GetCurrentUserHandler 
    : IRequestHandler<GetCurrentUserQuery, OperationResult<IdentityUserProfileDto>>
{
    private readonly IRepository<UserProfile> _userRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(
        IRepository<UserProfile> userRepository,
        UserManager<IdentityUser> userManager,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityUserProfileDto>> Handle(GetCurrentUserQuery request, 
        CancellationToken token)
    {
        var result = new OperationResult<IdentityUserProfileDto>();
        var identity = await _userManager.GetUserAsync(request.ClaimsPrincipal);

        var profile = await _userRepository
            .Get()
            .Filter(_ => _.Id == request.UserProfileId)
            .FirstAsync();

        result.Data = _mapper.Map<IdentityUserProfileDto>(profile);
        result.Data.UserName = identity.UserName;
        return result;
    }
}