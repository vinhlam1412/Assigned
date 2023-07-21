using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using HQSOFT.Common.HQTasks;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Shared;


namespace HQSOFT.Common.Blazor.Pages.Common.Tasks
{
    public partial class HQTasksListView
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private IReadOnlyList<HQTaskDto> HQTaskList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateHQTask { get; set; }
        private bool CanEditHQTask { get; set; }
        private bool CanDeleteHQTask { get; set; }
        private HQTaskCreateDto NewHQTask { get; set; }
        private Validations NewHQTaskValidations { get; set; } = new();
        private HQTaskUpdateDto EditingHQTask { get; set; }
        private Validations EditingHQTaskValidations { get; set; } = new();
        private Guid EditingHQTaskId { get; set; }
        private Modal CreateHQTaskModal { get; set; } = new();
        private Modal EditHQTaskModal { get; set; } = new();
        private GetHQTasksInput Filter { get; set; }
        private DataGridEntityActionsColumn<HQTaskDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "hQTask-create-tab";
        protected string SelectedEditTab = "hQTask-edit-tab";

        public HQTasksListView()
        {
            NewHQTask = new HQTaskCreateDto();
            EditingHQTask = new HQTaskUpdateDto();
            Filter = new GetHQTasksInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            HQTaskList = new List<HQTaskDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:HQTasks"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () => { await DownloadAsExcelAsync(); }, IconName.Download);

            Toolbar.AddButton(L["New"], () =>
            {
                NavigationManager.NavigateTo($"/Common/HQTasks/edit/{Guid.Empty}");
                return Task.CompletedTask;
            }, IconName.Add, requiredPolicyName: CommonPermissions.HQTasks.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateHQTask = await AuthorizationService
                .IsGrantedAsync(CommonPermissions.HQTasks.Create);
            CanEditHQTask = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQTasks.Edit);
            CanDeleteHQTask = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQTasks.Delete);
        }

        private async Task GetHQTasksAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HQTasksAppService.GetListAsync(Filter);
            HQTaskList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetHQTasksAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await HQTasksAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Configuration") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/configuration/h-qTasks/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<HQTaskDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetHQTasksAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateHQTaskModalAsync()
        {
            NewHQTask = new HQTaskCreateDto
            {
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now,


            };
            await NewHQTaskValidations.ClearAll();
            await CreateHQTaskModal.Show();
        }

        private async Task CloseCreateHQTaskModalAsync()
        {
            NewHQTask = new HQTaskCreateDto
            {
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now,


            };
            await CreateHQTaskModal.Hide();
        }

        private async Task OpenEditHQTaskModalAsync(HQTaskDto input)
        {
            var hQTask = await HQTasksAppService.GetAsync(input.Id);

            EditingHQTaskId = hQTask.Id;
            EditingHQTask = ObjectMapper.Map<HQTaskDto, HQTaskUpdateDto>(hQTask);
            await EditingHQTaskValidations.ClearAll();
            await EditHQTaskModal.Show();
        }


        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }
        private void GoToEditPage(HQTaskDto hqTask)
        {
            NavigationManager.NavigateTo($"/Common/HQTasks/edit/{hqTask.Id}");
        }



    }
}
