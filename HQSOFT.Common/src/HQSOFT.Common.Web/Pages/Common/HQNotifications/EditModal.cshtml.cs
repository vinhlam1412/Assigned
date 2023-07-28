using HQSOFT.Common.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQNotifications;

namespace HQSOFT.Common.Web.Pages.Common.HQNotifications
{
    public class EditModalModel : CommonPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public HQNotificationUpdateViewModel HQNotification { get; set; }

        private readonly IHQNotificationsAppService _hQNotificationsAppService;

        public EditModalModel(IHQNotificationsAppService hQNotificationsAppService)
        {
            _hQNotificationsAppService = hQNotificationsAppService;

            HQNotification = new();
        }

        public async Task OnGetAsync()
        {
            var hQNotification = await _hQNotificationsAppService.GetAsync(Id);
            HQNotification = ObjectMapper.Map<HQNotificationDto, HQNotificationUpdateViewModel>(hQNotification);

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _hQNotificationsAppService.UpdateAsync(Id, ObjectMapper.Map<HQNotificationUpdateViewModel, HQNotificationUpdateDto>(HQNotification));
            return NoContent();
        }
    }

    public class HQNotificationUpdateViewModel : HQNotificationUpdateDto
    {
    }
}