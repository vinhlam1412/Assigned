using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Web.Pages.Common.HQNotifications
{
    public class IndexModel : AbpPageModel
    {
        public string? IDParentFilter { get; set; }
        public string? ToUserFilter { get; set; }
        public string? FromUserFilter { get; set; }
        public string? NotiTitleFilter { get; set; }
        public string? NotiBodyFilter { get; set; }
        public string? TypeFilter { get; set; }
        [SelectItems(nameof(isReadBoolFilterItems))]
        public string isReadFilter { get; set; }

        public List<SelectListItem> isReadBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };

        private readonly IHQNotificationsAppService _hQNotificationsAppService;

        public IndexModel(IHQNotificationsAppService hQNotificationsAppService)
        {
            _hQNotificationsAppService = hQNotificationsAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}