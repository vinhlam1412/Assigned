namespace HQSOFT.Common.HQShares
{
    public static class HQShareConsts
    {
        private const string DefaultSorting = "{0}IDParent asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "HQShare." : string.Empty);
        }

    }
}