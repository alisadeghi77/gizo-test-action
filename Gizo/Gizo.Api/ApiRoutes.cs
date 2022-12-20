namespace Gizo.Api;
public static class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public static class User
    {
        public const string CheckIdentity = "checkIdentity";
        public const string Verify = "verify";
        public const string Profile = "profile";
        public const string Login = "login";
        public const string Registration = "registration";
        public const string Id = "{id}";
        public const string CurrentUser = "currentUser";
    }

    public static class Trip
    {
        public const string TripStart = "TripStart";
        public const string UploadChunks = "UploadChunks";
        public const string UploadComplete = "UploadComplete";
    }
}
