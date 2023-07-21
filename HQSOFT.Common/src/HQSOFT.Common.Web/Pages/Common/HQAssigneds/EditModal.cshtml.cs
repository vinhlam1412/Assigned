using HQSOFT.Common.Shared;
using Volo.Abp.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQAssigneds;

namespace HQSOFT.Common.Web.Pages.Common.HQAssigneds
{
    public class EditModalModel : CommonPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public HQAssignedUpdateViewModel HQAssigned { get; set; }

        public List<IdentityUserDto> IdentityUsers { get; set; }
        [BindProperty]
        public List<Guid> SelectedIdentityUserIds { get; set; }

        private readonly IHQAssignedsAppService _hQAssignedsAppService;

        public EditModalModel(IHQAssignedsAppService hQAssignedsAppService)
        {
            _hQAssignedsAppService = hQAssignedsAppService;

            HQAssigned = new();
        }

        public async Task OnGetAsync()
        {
            var hQAssignedWithNavigationPropertiesDto = await _hQAssignedsAppService.GetWithNavigationPropertiesAsync(Id);
            HQAssigned = ObjectMapper.Map<HQAssignedDto, HQAssignedUpdateViewModel>(hQAssignedWithNavigationPropertiesDto.HQAssigned);

            IdentityUsers = hQAssignedWithNavigationPropertiesDto.IdentityUsers;

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            HQAssigned.IdentityUserIds = SelectedIdentityUserIds;

            await _hQAssignedsAppService.UpdateAsync(Id, ObjectMapper.Map<HQAssignedUpdateViewModel, HQAssignedUpdateDto>(HQAssigned));
            return NoContent();
        }
    }

    public class HQAssignedUpdateViewModel : HQAssignedUpdateDto
    {
    }
}