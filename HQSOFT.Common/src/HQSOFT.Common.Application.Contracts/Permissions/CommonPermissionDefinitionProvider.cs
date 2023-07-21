using HQSOFT.Common.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HQSOFT.Common.Permissions;

public class CommonPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CommonPermissions.GroupName, L("Permission:Common"));

        var hQTaskPermission = myGroup.AddPermission(CommonPermissions.HQTasks.Default, L("Permission:HQTasks"));
        hQTaskPermission.AddChild(CommonPermissions.HQTasks.Create, L("Permission:Create"));
        hQTaskPermission.AddChild(CommonPermissions.HQTasks.Edit, L("Permission:Edit"));
        hQTaskPermission.AddChild(CommonPermissions.HQTasks.Delete, L("Permission:Delete"));

        var hQAssignedPermission = myGroup.AddPermission(CommonPermissions.HQAssigneds.Default, L("Permission:HQAssigneds"));
        hQAssignedPermission.AddChild(CommonPermissions.HQAssigneds.Create, L("Permission:Create"));
        hQAssignedPermission.AddChild(CommonPermissions.HQAssigneds.Edit, L("Permission:Edit"));
        hQAssignedPermission.AddChild(CommonPermissions.HQAssigneds.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CommonResource>(name);
    }
}