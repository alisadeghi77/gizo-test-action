namespace Gizo.Api;
public static class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public static class UserProfiles
    {
        public const string IdRoute = "{id}";
    }

    public static class Identity
    {
        public const string Login = "login";
        public const string Registration = "registration";
        public const string IdentityById = "{identityUserId}";
        public const string CurrentUser = "currentuser";
    }

    public static class ClientIdentity 
    {
        public const string CheckIdentity = "CheckIdentity";
        public const string Verify = "Verify";
        public const string UpdateUserProfile = "UpdateUserProfile";
    }
}
