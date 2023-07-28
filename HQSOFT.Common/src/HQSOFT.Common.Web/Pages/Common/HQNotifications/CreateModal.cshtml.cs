using HQSOFT.Common.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HQSOFT.Common.HQNotifications;

namespace HQSOFT.Common.Web.Pages.Common.HQNotifications
{
    public class CreateModalModel : CommonPageModel
    {
        [BindProperty]
        public HQNotificationCreateViewModel HQNotification { get; set; }

        private readonly IHQNotificationsAppService _hQNotificationsAppService;

        public CreateModalModel(IHQNotificationsAppService hQNotificationsAppService)
        {
            _hQNotificationsAppService = hQNotificationsAppService;

            HQNotification = new();
        }

        public async Task OnGetAsync()
        {
            HQNotification = new HQNotificationCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _hQNotificationsAppService.CreateAsync(ObjectMapper.Map<HQNotificationCreateViewModel, HQNotificationCreateDto>(HQNotification));
            return NoContent();
        }
    }

    public class HQNotificationCreateViewModel : HQNotificationCreateDto
    {
    }
}