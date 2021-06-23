namespace Zoobook.Shared
{
    public static class Constants
    {
        #region Api Routes
        public const string ApiRouteEmployees = "employees";
        #endregion

        #region CorsPolicy
        public const string ZoobookPolicyName = "ZoobookSystemLLCPolicy";
        #endregion

        #region Documentation
        public const string DocumentVersion = "v1";

        public const string DocumentTitle = "Zoobook Systems LLC API 1.0";

        public const string DocumentDescription = "Documentation for Zoobook Systems LLC API.";
        #endregion

        #region Dates
        public const string DefaultDateTimeFormat = "uuuu'-'MM'-'dd'T'HH':'mm':'ss;FFFFFF'Z'";

        public const string DefaultDateFormat = "yyyy-MM-dd";

        public const string DefaultTimeFormat = "HH:mm:ss";

        public const int DaysInWeek = 7;

        public const int BusinessDaysInWeek = 5;

        public const int FullDayHours = 8;

        public const int HalfDayHours = 4;

        public const int MinutesPerHour = 60;

        public const int SecondsPerHour = 3600;

        public const int HoursPerDay = 24;

        public const string DefaultTimeZone = "Australia/Sydney";

        public const string DefaultCountyCode = "AUS-NSW";
        #endregion

        #region Paging
        public const int DefaultPageIndex = 1;

        public const int DefaultPageSize = 0;

        public const int MaximumPageSize = 1000;

        public const int MaximumDescendantsCount = 5;
        #endregion

        #region Common
        public const string EnumNone = "None";
        #endregion
    }
}
