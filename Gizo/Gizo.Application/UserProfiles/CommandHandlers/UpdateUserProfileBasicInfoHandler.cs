using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.UserProfiles.Commands;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Contracts.Repository;
using Gizo.Domain.Exceptions;
using MediatR;

namespace Gizo.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<UserProfile>>
{
    private readonly IRepository<UserProfile> _userRepository;
    private readonly IUnitOfWork _uow;

    public UpdateUserProfileBasicInfoHandler(
        IRepository<UserProfile> userRepository,
        IUnitOfWork uow)
    {
        _userRepository = userRepository;
        _uow = uow;
    }
    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfoCommand request,
        CancellationToken token)
    {
        var result = new OperationResult<UserProfile>();

        try
        {
            var userProfile = await _userRepository
                .Get()
                .Filter(_ => _.Id == request.UserProfileId)
                .FirstAsync();

            if (userProfile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

            userProfile.UpdateBasicInfo(basicInfo);

             _userRepository.Update(userProfile);
            await _uow.SaveChangesAsync(token);

            result.Data = userProfile;
            return result;
        }

        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, e));
        }

        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}
