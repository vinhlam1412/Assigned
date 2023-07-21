namespace HQSOFT.Common.HQAssigneds
{
    public static class HQAssignedConsts
    {
        private const string DefaultSorting = "{0}IDParent asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "HQAssigned." : string.Empty);
        }

    }
}