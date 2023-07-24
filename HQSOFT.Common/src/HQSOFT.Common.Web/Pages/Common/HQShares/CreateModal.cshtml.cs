using HQSOFT.Common.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HQSOFT.Common.HQShares;

namespace HQSOFT.Common.Web.Pages.Common.HQShares
{
    public class CreateModalModel : CommonPageModel
    {
        [BindProperty]
        public HQShareCreateViewModel HQShare { get; set; }

        [BindProperty]
        public List<Guid> SelectedIdentityUserIds { get; set; }

        private readonly IHQSharesAppService _hQSharesAppService;

        public CreateModalModel(IHQSharesAppService hQSharesAppService)
        {
            _hQSharesAppService = hQSharesAppService;

            HQShare = new();
        }

        public async Task OnGetAsync()
        {
            HQShare = new HQShareCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            HQShare.IdentityUserIds = SelectedIdentityUserIds;

            await _hQSharesAppService.CreateAsync(ObjectMapper.Map<HQShareCreateViewModel, HQShareCreateDto>(HQShare));
            return NoContent();
        }
    }

    public class HQShareCreateViewModel : HQShareCreateDto
    {
    }
}