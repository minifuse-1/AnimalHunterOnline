
using Unity.Collections.LowLevel.Unsafe;

public static class AppConstants
{
    public static class Api
    {
        public const string LoginEndpoint = "/api/auth/login";
        public const string ProfileEndpoint = "/api/profiles/me";
    }

    public static class Auth
    {
        public const string DataPath = "auth_data.json";
        
    }

    public static class MatchHistory
    {
        public const string FormatVersion = "1.0.0";
        public const string DataPath = "match_history.json";
    }
}
