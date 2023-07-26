using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Web.Pages.Common.HQShares
{
    public class IndexModel : AbpPageModel
    {
        public string? IDParentFilter { get; set; }
        [SelectItems(nameof(CanReadBoolFilterItems))]
        public string CanReadFilter { get; set; }

        public List<SelectListItem> CanReadBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(CanWriteBoolFilterItems))]
        public string CanWriteFilter { get; set; }

        public List<SelectListItem> CanWriteBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(CanSubmitBoolFilterItems))]
        public string CanSubmitFilter { get; set; }

        public List<SelectListItem> CanSubmitBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(CanShareBoolFilterItems))]
        public string CanShareFilter { get; set; }

        public List<SelectListItem> CanShareBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(IdentityUserLookupList))]
        public Guid? IdentityUserIdFilter { get; set; }
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        private readonly IHQSharesAppService _hQSharesAppService;

        public IndexModel(IHQSharesAppService hQSharesAppService)
        {
            _hQSharesAppService = hQSharesAppService;
        }

        public async Task OnGetAsync()
        {
            IdentityUserLookupList.AddRange((
                    await _hQSharesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            await Task.CompletedTask;
        }
    }
}