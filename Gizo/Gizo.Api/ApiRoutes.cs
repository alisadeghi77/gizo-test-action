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
        public const string CarModels = "carModels";
        public const string CarModelDetail = "carModels/{id}";
        public const string CarModel = "carModel";
        public const string CarModelSelect = "carModel/select";
    }

    public static class Trip
    {
        public const string TripDetail = "{id}";
        public const string CreateTrip = "createTrip";
        public const string UploadStart = "uploadStart";
        public const string UploadChunks = "uploadChunks";
        public const string FileChunkStatus = "fileChunkStatus";
    }
}
