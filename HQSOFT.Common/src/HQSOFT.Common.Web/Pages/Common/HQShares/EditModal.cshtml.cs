using HQSOFT.Common.Shared;
using Volo.Abp.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.HQShares;

namespace HQSOFT.Common.Web.Pages.Common.HQShares
{
    public class EditModalModel : CommonPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public HQShareUpdateViewModel HQShare { get; set; }

        public List<IdentityUserDto> IdentityUsers { get; set; }
        [BindProperty]
        public List<Guid> SelectedIdentityUserIds { get; set; }

        private readonly IHQSharesAppService _hQSharesAppService;

        public EditModalModel(IHQSharesAppService hQSharesAppService)
        {
            _hQSharesAppService = hQSharesAppService;

            HQShare = new();
        }

        public async Task OnGetAsync()
        {
            var hQShareWithNavigationPropertiesDto = await _hQSharesAppService.GetWithNavigationPropertiesAsync(Id);
            HQShare = ObjectMapper.Map<HQShareDto, HQShareUpdateViewModel>(hQShareWithNavigationPropertiesDto.HQShare);

            IdentityUsers = hQShareWithNavigationPropertiesDto.IdentityUsers;

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            HQShare.IdentityUserIds = SelectedIdentityUserIds;

            await _hQSharesAppService.UpdateAsync(Id, ObjectMapper.Map<HQShareUpdateViewModel, HQShareUpdateDto>(HQShare));
            return NoContent();
        }
    }

    public class HQShareUpdateViewModel : HQShareUpdateDto
    {
    }
}