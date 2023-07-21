using HQSOFT.Common.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HQSOFT.Common.HQAssigneds;

namespace HQSOFT.Common.Web.Pages.Common.HQAssigneds
{
    public class CreateModalModel : CommonPageModel
    {
        [BindProperty]
        public HQAssignedCreateViewModel HQAssigned { get; set; }

        [BindProperty]
        public List<Guid> SelectedIdentityUserIds { get; set; }

        private readonly IHQAssignedsAppService _hQAssignedsAppService;

        public CreateModalModel(IHQAssignedsAppService hQAssignedsAppService)
        {
            _hQAssignedsAppService = hQAssignedsAppService;

            HQAssigned = new();
        }

        public async Task OnGetAsync()
        {
            HQAssigned = new HQAssignedCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            HQAssigned.IdentityUserIds = SelectedIdentityUserIds;

            await _hQAssignedsAppService.CreateAsync(ObjectMapper.Map<HQAssignedCreateViewModel, HQAssignedCreateDto>(HQAssigned));
            return NoContent();
        }
    }

    public class HQAssignedCreateViewModel : HQAssignedCreateDto
    {
    }
}