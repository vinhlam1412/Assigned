using HQSOFT.Common.Permissions;
using HQSOFT.Common.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Volo.Abp.Authorization.Permissions;

namespace HQSOFT.Common.Web.Menus;

public class CommonMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return;
        }

        var moduleMenu = AddModuleMenuItem(context); //Do not delete `moduleMenu` variable as it will be used by ABP Suite!

        AddMenuItemHQTasks(context, moduleMenu);

        AddMenuItemHQAssigneds(context, moduleMenu);
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            CommonMenus.Prefix,
            displayName: "Common",
            "~/Common",
            icon: "fa fa-globe");

        //Add main menu items.
        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
    private static void AddMenuItemHQTasks(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.HQTasks,
                context.GetLocalizer<CommonResource>()["Menu:HQTasks"],
                "/Common/HQTasks",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.HQTasks.Default
            )
        );
    }

    private static void AddMenuItemHQAssigneds(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.CommonMenus.HQAssigneds,
                context.GetLocalizer<CommonResource>()["Menu:HQAssigneds"],
                "/Common/HQAssigneds",
                icon: "fa fa-file-alt",
                requiredPermissionName: CommonPermissions.HQAssigneds.Default
            )
        );
    }
}