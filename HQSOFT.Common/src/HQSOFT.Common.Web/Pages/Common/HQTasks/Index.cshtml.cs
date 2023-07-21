using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using HQSOFT.Common.HQTasks;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Web.Pages.Common.HQTasks
{
    public class IndexModel : AbpPageModel
    {
        public string? SubjectFilter { get; set; }
        public string? ProjectFilter { get; set; }
        public StatusTask? StatusFilter { get; set; }
        public PriorityTask? PriorityFilter { get; set; }
        public DateTime? ExpectedStartDateFilterMin { get; set; }

        public DateTime? ExpectedStartDateFilterMax { get; set; }
        public DateTime? ExpectedEndDateFilterMin { get; set; }

        public DateTime? ExpectedEndDateFilterMax { get; set; }
        public double? ExpectedTimeFilterMin { get; set; }

        public double? ExpectedTimeFilterMax { get; set; }
        public double? ProcessFilterMin { get; set; }

        public double? ProcessFilterMax { get; set; }

        private readonly IHQTasksAppService _hQTasksAppService;

        public IndexModel(IHQTasksAppService hQTasksAppService)
        {
            _hQTasksAppService = hQTasksAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}