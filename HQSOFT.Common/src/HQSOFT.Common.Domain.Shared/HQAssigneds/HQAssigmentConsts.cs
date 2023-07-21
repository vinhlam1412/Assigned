namespace HQSOFT.Configuration.HQAssigments
{
    public static class HQAssigmentConsts
    {
        private const string DefaultSorting = "{0}Completeby asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "HQAssigment." : string.Empty);
        }

    }
}