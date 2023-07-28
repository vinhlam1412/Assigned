using HQSOFT.Common.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace HQSOFT.Common.Blazor.Toolbars
{
    public class CommonToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == StandardToolbars.Main)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(Notification)));
            }

            return Task.CompletedTask;
        }
    }
}
