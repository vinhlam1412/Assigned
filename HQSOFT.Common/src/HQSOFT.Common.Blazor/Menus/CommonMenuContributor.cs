using HQSOFT.Common.Permissions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using HQSOFT.Common.Localization;
using Volo.Abp.UI.Navigation;

namespace HQSOFT.Common.Blazor.Menus;

public class CommonMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }

        var moduleMenu = AddModuleMenuItem(context);
        AddMenuItemHQTasks(context, moduleMenu);

        AddMenuItemHQAssigneds(context, moduleMenu);
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        var l = context.GetLocalizer<CommonResource>();

        context.Menu.AddItem(new ApplicationMenuItem(CommonMenus.Prefix, displayName: "Sample Page", "/Common", icon: "fa fa-globe"));

        await Task.CompletedTask;
    }
    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            CommonMenus.Prefix,
            context.GetLocalizer<CommonResource>()["Menu:Common"],
            icon: "fa fa-folder"
        );

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