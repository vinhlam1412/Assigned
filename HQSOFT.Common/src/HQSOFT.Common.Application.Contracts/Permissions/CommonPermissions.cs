using Volo.Abp.Reflection;

namespace HQSOFT.Common.Permissions;

public class CommonPermissions
{
    public const string GroupName = "Common";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CommonPermissions));
    }

    public static class HQTasks
    {
        public const string Default = GroupName + ".HQTasks";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class HQAssigneds
    {
        public const string Default = GroupName + ".HQAssigneds";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}