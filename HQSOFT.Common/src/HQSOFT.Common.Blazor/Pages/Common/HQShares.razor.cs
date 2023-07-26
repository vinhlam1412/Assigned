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
using HQSOFT.Common.HQShares;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Blazor.Pages.Common
{
    public partial class HQShares
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<HQShareWithNavigationPropertiesDto> HQShareList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateHQShare { get; set; }
        private bool CanEditHQShare { get; set; }
        private bool CanDeleteHQShare { get; set; }
        private HQShareCreateDto NewHQShare { get; set; }
        private Validations NewHQShareValidations { get; set; } = new();
        private HQShareUpdateDto EditingHQShare { get; set; }
        private Validations EditingHQShareValidations { get; set; } = new();
        private Guid EditingHQShareId { get; set; }
        private Modal CreateHQShareModal { get; set; } = new();
        private Modal EditHQShareModal { get; set; } = new();
        private GetHQSharesInput Filter { get; set; }
        private DataGridEntityActionsColumn<HQShareWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "hQShare-create-tab";
        protected string SelectedEditTab = "hQShare-edit-tab";
        private IReadOnlyList<LookupDto<Guid>> UsersCollection { get; set; } = new List<LookupDto<Guid>>();

        public HQShares()
        {
            NewHQShare = new HQShareCreateDto();
            EditingHQShare = new HQShareUpdateDto();
            Filter = new GetHQSharesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            HQShareList = new List<HQShareWithNavigationPropertiesDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:HQShares"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewHQShare"], async () =>
            {
                await OpenCreateHQShareModalAsync();
            }, IconName.Add, requiredPolicyName: CommonPermissions.HQShares.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateHQShare = await AuthorizationService
                .IsGrantedAsync(CommonPermissions.HQShares.Create);
            CanEditHQShare = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQShares.Edit);
            CanDeleteHQShare = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQShares.Delete);
        }

        private async Task GetHQSharesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HQSharesAppService.GetListAsync(Filter);
            HQShareList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetHQSharesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await HQSharesAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Common") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/common/h-qShares/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<HQShareWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetHQSharesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateHQShareModalAsync()
        {
            NewHQShare = new HQShareCreateDto{
                
                
            };
            await NewHQShareValidations.ClearAll();
            await CreateHQShareModal.Show();
        }

        private async Task CloseCreateHQShareModalAsync()
        {
            NewHQShare = new HQShareCreateDto{
                
                
            };
            await CreateHQShareModal.Hide();
        }

        private async Task OpenEditHQShareModalAsync(HQShareWithNavigationPropertiesDto input)
        {
            var hQShare = await HQSharesAppService.GetWithNavigationPropertiesAsync(input.HQShare.Id);
            
            EditingHQShareId = hQShare.HQShare.Id;
            EditingHQShare = ObjectMapper.Map<HQShareDto, HQShareUpdateDto>(hQShare.HQShare);
            await EditingHQShareValidations.ClearAll();
            await EditHQShareModal.Show();
        }

        private async Task DeleteHQShareAsync(HQShareWithNavigationPropertiesDto input)
        {
            await HQSharesAppService.DeleteAsync(input.HQShare.Id);
            await GetHQSharesAsync();
        }

        private async Task CreateHQShareAsync()
        {
            try
            {
                if (await NewHQShareValidations.ValidateAll() == false)
                {
                    return;
                }

                await HQSharesAppService.CreateAsync(NewHQShare);
                await GetHQSharesAsync();
                await CloseCreateHQShareModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditHQShareModalAsync()
        {
            await EditHQShareModal.Hide();
        }

        private async Task UpdateHQShareAsync()
        {
            try
            {
                if (await EditingHQShareValidations.ValidateAll() == false)
                {
                    return;
                }

                await HQSharesAppService.UpdateAsync(EditingHQShareId, EditingHQShare);
                await GetHQSharesAsync();
                await EditHQShareModal.Hide();                
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
        

        private async Task GetIdentityUserCollectionLookupAsync(string? newValue = null)
        {
            UsersCollection = (await HQSharesAppService.GetIdentityUserLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

    }
}
