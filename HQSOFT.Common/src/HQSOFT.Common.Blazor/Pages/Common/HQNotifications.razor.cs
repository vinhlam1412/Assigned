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
using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Blazor.Pages.Common
{
    public partial class HQNotifications
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<HQNotificationDto> HQNotificationList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateHQNotification { get; set; }
        private bool CanEditHQNotification { get; set; }
        private bool CanDeleteHQNotification { get; set; }
        private HQNotificationCreateDto NewHQNotification { get; set; }
        private Validations NewHQNotificationValidations { get; set; } = new();
        private HQNotificationUpdateDto EditingHQNotification { get; set; }
        private Validations EditingHQNotificationValidations { get; set; } = new();
        private Guid EditingHQNotificationId { get; set; }
        private Modal CreateHQNotificationModal { get; set; } = new();
        private Modal EditHQNotificationModal { get; set; } = new();
        private GetHQNotificationsInput Filter { get; set; }
        private DataGridEntityActionsColumn<HQNotificationDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "hQNotification-create-tab";
        protected string SelectedEditTab = "hQNotification-edit-tab";
        
        public HQNotifications()
        {
            NewHQNotification = new HQNotificationCreateDto();
            EditingHQNotification = new HQNotificationUpdateDto();
            Filter = new GetHQNotificationsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            HQNotificationList = new List<HQNotificationDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:HQNotifications"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewHQNotification"], async () =>
            {
                await OpenCreateHQNotificationModalAsync();
            }, IconName.Add, requiredPolicyName: CommonPermissions.HQNotifications.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateHQNotification = await AuthorizationService
                .IsGrantedAsync(CommonPermissions.HQNotifications.Create);
            CanEditHQNotification = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQNotifications.Edit);
            CanDeleteHQNotification = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQNotifications.Delete);
        }

        private async Task GetHQNotificationsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HQNotificationsAppService.GetListAsync(Filter);
            HQNotificationList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetHQNotificationsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await HQNotificationsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Common") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/common/h-qNotifications/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<HQNotificationDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetHQNotificationsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateHQNotificationModalAsync()
        {
            NewHQNotification = new HQNotificationCreateDto{
                
                
            };
            await NewHQNotificationValidations.ClearAll();
            await CreateHQNotificationModal.Show();
        }

        private async Task CloseCreateHQNotificationModalAsync()
        {
            NewHQNotification = new HQNotificationCreateDto{
                
                
            };
            await CreateHQNotificationModal.Hide();
        }

        private async Task OpenEditHQNotificationModalAsync(HQNotificationDto input)
        {
            var hQNotification = await HQNotificationsAppService.GetAsync(input.Id);
            
            EditingHQNotificationId = hQNotification.Id;
            EditingHQNotification = ObjectMapper.Map<HQNotificationDto, HQNotificationUpdateDto>(hQNotification);
            await EditingHQNotificationValidations.ClearAll();
            await EditHQNotificationModal.Show();
        }

        private async Task DeleteHQNotificationAsync(HQNotificationDto input)
        {
            await HQNotificationsAppService.DeleteAsync(input.Id);
            await GetHQNotificationsAsync();
        }

        private async Task CreateHQNotificationAsync()
        {
            try
            {
                if (await NewHQNotificationValidations.ValidateAll() == false)
                {
                    return;
                }

                await HQNotificationsAppService.CreateAsync(NewHQNotification);
                await GetHQNotificationsAsync();
                await CloseCreateHQNotificationModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditHQNotificationModalAsync()
        {
            await EditHQNotificationModal.Hide();
        }

        private async Task UpdateHQNotificationAsync()
        {
            try
            {
                if (await EditingHQNotificationValidations.ValidateAll() == false)
                {
                    return;
                }

                await HQNotificationsAppService.UpdateAsync(EditingHQNotificationId, EditingHQNotification);
                await GetHQNotificationsAsync();
                await EditHQNotificationModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }
        

    }
}
