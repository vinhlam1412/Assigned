namespace HQSOFT.Common.HQNotifications
{
    public static class HQNotificationConsts
    {
        private const string DefaultSorting = "{0}IDParent asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "HQNotification." : string.Empty);
        }

    }
}