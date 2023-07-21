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
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Blazor.Pages.Common
{
    public partial class HQAssigneds
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<HQAssignedWithNavigationPropertiesDto> HQAssignedList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateHQAssigned { get; set; }
        private bool CanEditHQAssigned { get; set; }
        private bool CanDeleteHQAssigned { get; set; }
        private HQAssignedCreateDto NewHQAssigned { get; set; }
        private Validations NewHQAssignedValidations { get; set; } = new();
        private HQAssignedUpdateDto EditingHQAssigned { get; set; }
        private Validations EditingHQAssignedValidations { get; set; } = new();
        private Guid EditingHQAssignedId { get; set; }
        private Modal CreateHQAssignedModal { get; set; } = new();
        private Modal EditHQAssignedModal { get; set; } = new();
        private GetHQAssignedsInput Filter { get; set; }
        private DataGridEntityActionsColumn<HQAssignedWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "hQAssigned-create-tab";
        protected string SelectedEditTab = "hQAssigned-edit-tab";
        private IReadOnlyList<LookupDto<Guid>> IdentityUsers { get; set; } = new List<LookupDto<Guid>>();
        
        private string SelectedIdentityUserId { get; set; }
        
        private string SelectedIdentityUserText { get; set; }

        private List<LookupDto<Guid>> SelectedIdentityUsers { get; set; } = new List<LookupDto<Guid>>();
        public HQAssigneds()
        {
            NewHQAssigned = new HQAssignedCreateDto();
            EditingHQAssigned = new HQAssignedUpdateDto();
            Filter = new GetHQAssignedsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            HQAssignedList = new List<HQAssignedWithNavigationPropertiesDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:HQAssigneds"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewHQAssigned"], async () =>
            {
                await OpenCreateHQAssignedModalAsync();
            }, IconName.Add, requiredPolicyName: CommonPermissions.HQAssigneds.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateHQAssigned = await AuthorizationService
                .IsGrantedAsync(CommonPermissions.HQAssigneds.Create);
            CanEditHQAssigned = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQAssigneds.Edit);
            CanDeleteHQAssigned = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.HQAssigneds.Delete);
        }

        private async Task GetHQAssignedsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HQAssignedsAppService.GetListAsync(Filter);
            HQAssignedList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetHQAssignedsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await HQAssignedsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Common") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/common/h-qAssigneds/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<HQAssignedWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetHQAssignedsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateHQAssignedModalAsync()
        {
            SelectedIdentityUsers = new List<LookupDto<Guid>>();
            

            NewHQAssigned = new HQAssignedCreateDto{
                Completeby = DateTime.Now,

                
            };
            await NewHQAssignedValidations.ClearAll();
            await CreateHQAssignedModal.Show();
        }

        private async Task CloseCreateHQAssignedModalAsync()
        {
            NewHQAssigned = new HQAssignedCreateDto{
                Completeby = DateTime.Now,

                
            };
            await CreateHQAssignedModal.Hide();
        }

        private async Task OpenEditHQAssignedModalAsync(HQAssignedWithNavigationPropertiesDto input)
        {
            var hQAssigned = await HQAssignedsAppService.GetWithNavigationPropertiesAsync(input.HQAssigned.Id);
            
            EditingHQAssignedId = hQAssigned.HQAssigned.Id;
            EditingHQAssigned = ObjectMapper.Map<HQAssignedDto, HQAssignedUpdateDto>(hQAssigned.HQAssigned);
            SelectedIdentityUsers = hQAssigned.IdentityUsers.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.Email}).ToList();

            await EditingHQAssignedValidations.ClearAll();
            await EditHQAssignedModal.Show();
        }

        private async Task DeleteHQAssignedAsync(HQAssignedWithNavigationPropertiesDto input)
        {
            await HQAssignedsAppService.DeleteAsync(input.HQAssigned.Id);
            await GetHQAssignedsAsync();
        }

        private async Task CreateHQAssignedAsync()
        {
            try
            {
                if (await NewHQAssignedValidations.ValidateAll() == false)
                {
                    return;
                }
                NewHQAssigned.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();


                await HQAssignedsAppService.CreateAsync(NewHQAssigned);
                await GetHQAssignedsAsync();
                await CloseCreateHQAssignedModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditHQAssignedModalAsync()
        {
            await EditHQAssignedModal.Hide();
        }

        private async Task UpdateHQAssignedAsync()
        {
            try
            {
                if (await EditingHQAssignedValidations.ValidateAll() == false)
                {
                    return;
                }
                EditingHQAssigned.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();


                await HQAssignedsAppService.UpdateAsync(EditingHQAssignedId, EditingHQAssigned);
                await GetHQAssignedsAsync();
                await EditHQAssignedModal.Hide();                
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
        

        private async Task GetIdentityUserLookupAsync(string? newValue = null)
        {
            IdentityUsers = (await HQAssignedsAppService.GetIdentityUserLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private void AddIdentityUser()
        {
            if (SelectedIdentityUserId.IsNullOrEmpty())
            {
                return;
            }
            
            if (SelectedIdentityUsers.Any(p => p.Id.ToString() == SelectedIdentityUserId))
            {
                UiMessageService.Warn(L["ItemAlreadyAdded"]);
                return;
            }

            SelectedIdentityUsers.Add(new LookupDto<Guid>
            {
                Id = Guid.Parse(SelectedIdentityUserId),
                DisplayName = SelectedIdentityUserText
            });
        }

    }
}
