using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class User : IdentityUser<long>, IEntity, ICreateDate, IOptionalModifiedDate<long>
{
    private readonly List<UserCarModel> _userCarModels = new();
    private readonly List<UserLocation> _userLocations = new();

    public User()
    {
        CreateDate = DateTime.UtcNow;
    }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public DateTime CreateDate { get; private set; }

    public DateTime? ModifyDate { get; private set; }

    public long? ModifierId { get; private set; }

    public IReadOnlyCollection<Trip> Trips { get; private set; } = null!;

    private readonly List<UserVerificationCode> _userVerificationCodes = new();

    public IReadOnlyCollection<UserVerificationCode> UserVerificationCodes => _userVerificationCodes;

    public IReadOnlyCollection<UserCarModel> UserCarModels => _userCarModels;

    public IReadOnlyCollection<UserLocation> UserLocations => _userLocations;

    public void UpdateProfile(long userId, string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        NormalizedEmail = email.ToUpper();
        ModifyDate = DateTime.UtcNow;
        ModifierId = userId;
    }

    public List<UserCarModel> AddCar(long carModelId, string license)
    {
        var carModel = new UserCarModel(Id, carModelId, license);
        _userCarModels.Add(carModel);

        if (_userCarModels.Count == 1)
            carModel.SelectCarModel();

        return _userCarModels;
    }

    public List<UserCarModel> RemoveCar(long userCarModelId)
    {
        var userCarModel = _userCarModels.FirstOrDefault(_ => _.Id == userCarModelId);

        if (userCarModel == null)
        {
            throw new NotFoundException("Car model");
        }

        _userCarModels.Remove(userCarModel);

        return _userCarModels;
    }

    public User UpdateUserCarModel(User user, long userCarModelId, string license)
    {
        var userCarModel = _userCarModels.FirstOrDefault(_ => _.Id == userCarModelId);

        if (userCarModel == null)
        {
            throw new NotFoundException("User car model");
        }

        userCarModel.SetLicense(license);

        return user;
    }

    public UserCarModel? FindUserCarModel(long carModelId)
    {
        return UserCarModels.FirstOrDefault(_ => _.CarModelId == carModelId);
    }

    public bool IsUserCarModelDuplicate(long carModelId, string license)
    {
        return _userCarModels.Any(_ => _.IsDuplicate(Id, carModelId, license));
    }

    public bool ValidateCode(TimeSpan expirationDurationTime, string code, VerificationType verificationType)
    {
        var data = DateTime.UtcNow.Add(expirationDurationTime * -1);
        var result = UserVerificationCodes
            .Where(_ => _.CreateDate >= data)
            .Where(_ => _.VerificationType == verificationType)
            .Any(_ => _.Code == code);

        if (result)
        {
            _userVerificationCodes.RemoveAll(_ => _.VerificationType == verificationType);
        }

        return result;
    }

    public UserVerificationCode? GetValidCode(TimeSpan expirationDurationTime, VerificationType verificationType)
    {
        var date = DateTime.UtcNow.Add(expirationDurationTime * -1);

        var verifyCode = UserVerificationCodes
            .Where(_ => _.CreateDate >= date)
            .Where(_ => _.VerificationType == verificationType)
            .MinBy(_ => _.CreateDate);

        return verifyCode;
    }

    public UserVerificationCode CreateCode(bool useSampleCode, VerificationType verificationType)
    {
        var verifyCode = UserVerificationCode.Create(Id, useSampleCode, verificationType);
        _userVerificationCodes.Add(verifyCode);
        return verifyCode;
    }
    public static User CreateUserByPhoneNumber(string phoneNumber)
    {
        return new User()
        {
            PhoneNumber = phoneNumber,
            UserName = phoneNumber
        };
    }

    public User SelectCar(long userCarModelId)
    {
        var userCarModel = _userCarModels.FirstOrDefault(t => t.Id == userCarModelId);
        if (userCarModel is null)
        {
            throw new NotFoundException("User car model");
        }

        _userCarModels.ForEach(u => u.RemoveSelectCarModel());
        userCarModel.SelectCarModel();

        return this;
    }
}
