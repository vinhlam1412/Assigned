using HQSOFT.Common.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQTasks;

namespace HQSOFT.Common.Web.Pages.Common.HQTasks
{
    public class EditModalModel : CommonPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public HQTaskUpdateViewModel HQTask { get; set; }

        private readonly IHQTasksAppService _hQTasksAppService;

        public EditModalModel(IHQTasksAppService hQTasksAppService)
        {
            _hQTasksAppService = hQTasksAppService;

            HQTask = new();
        }

        public async Task OnGetAsync()
        {
            var hQTask = await _hQTasksAppService.GetAsync(Id);
            HQTask = ObjectMapper.Map<HQTaskDto, HQTaskUpdateViewModel>(hQTask);

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _hQTasksAppService.UpdateAsync(Id, ObjectMapper.Map<HQTaskUpdateViewModel, HQTaskUpdateDto>(HQTask));
            return NoContent();
        }
    }

    public class HQTaskUpdateViewModel : HQTaskUpdateDto
    {
    }
}