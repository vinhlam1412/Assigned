using HQSOFT.Configuration.HQAssigments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Web.Pages.Common.HQAssigneds
{
    public class IndexModel : AbpPageModel
    {
        public string? IDParentFilter { get; set; }
        public DateTime? CompletebyFilterMin { get; set; }

        public DateTime? CompletebyFilterMax { get; set; }
        public PriorityAssign? PriorityFilter { get; set; }
        public string? CommentFilter { get; set; }

        private readonly IHQAssignedsAppService _hQAssignedsAppService;

        public IndexModel(IHQAssignedsAppService hQAssignedsAppService)
        {
            _hQAssignedsAppService = hQAssignedsAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}