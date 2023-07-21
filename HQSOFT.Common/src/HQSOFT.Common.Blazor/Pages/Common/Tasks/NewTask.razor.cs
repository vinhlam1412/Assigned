using AutoMapper.Internal.Mappers;
using Blazorise;
using HQSOFT.Common.HQTasks;
using HQSOFT.Common.Permissions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.ObjectMapping;

namespace HQSOFT.Common.Blazor.Pages.Common.Tasks
{
    public partial class NewTask
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
        private HQTaskUpdateDto EditingHQTask { get; set; }
        private Validations EditingHQTaskValidations { get; set; } = new();
        private Guid EditingHQTaskId { get; set; }
        private GetHQTasksInput Filter { get; set; }

        private readonly IUiMessageService _messageService;

        Blazorise.Visibility isVisible;

        [Parameter]
        public string Id { get; set; }

        public NewTask(IUiMessageService messageService)
        {
            EditingHQTask = new HQTaskUpdateDto();
            EditingHQTask.ConcurrencyStamp = string.Empty;
            _messageService = messageService;
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            //isVisible = Visibility.Invisible;


            //await SetPermissionsAsync();


            EditingHQTaskId = Guid.Parse(Id);
            if (EditingHQTaskId != Guid.Empty)
            {
                //isVisible = Visibility.Visible;
                var attributeID = await HQTasksAppService.GetAsync(EditingHQTaskId);
                EditingHQTask = ObjectMapper.Map<HQTaskDto, HQTaskUpdateDto>(attributeID);
            }
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Attributes"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Back"], () =>
            {
                GoToListPage();
                return Task.CompletedTask;
            },
            IconName.Undo,
            Color.Secondary);

            Toolbar.AddButton(L["New"], async () =>
            {
                await SaveCsAttributeAsync(true);
            },
            IconName.Add,
            requiredPolicyName: CommonPermissions.HQTasks.Create);

            Toolbar.AddButton(L["Save"], async () =>
            {
                await SaveCsAttributeAsync(false);
            },
            IconName.Save,
            requiredPolicyName: CommonPermissions.HQTasks.Edit);

            Toolbar.AddButton(L["Delete"], async () =>
            {
                var confirmed = await _messageService.Confirm(L["DeleteConfirmationMessage"]);
                if (confirmed)
                {
                    await DeleteHQTaskAsync(EditingHQTaskId);
                }
            }, IconName.Delete,
            Color.Danger,
            requiredPolicyName: CommonPermissions.HQTasks.Delete);

            return ValueTask.CompletedTask;
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
        public async Task SaveCsAttributeAsync(bool isNewNext)
        {
            try
            {
                if (EditingHQTaskId == Guid.Empty)
                {
                    await CreateHQTaskAsync();
                    isVisible = Visibility.Visible;
                }
                else
                {
                    await UpdateHQTaskAsync();
                }
                if (isNewNext)
                {
                    NavigationManager.NavigateTo($"/Common/HQTasks/edit/{Guid.Empty}", true);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private async Task DeleteHQTaskAsync(Guid Id)
        {
            await HQTasksAppService.DeleteAsync(Id);
            await GetHQTasksAsync();
        }

        private async Task CreateHQTaskAsync()
        {
            try
            {
                if (await EditingHQTaskValidations.ValidateAll() == false)
                {
                    return;
                }
                HQTaskCreateDto hqCreateDto = ObjectMapper.Map<HQTaskUpdateDto, HQTaskCreateDto>(EditingHQTask);
                var hqDto = await HQTasksAppService.CreateAsync(hqCreateDto);

                EditingHQTaskId = hqDto.Id;
                EditingHQTask = ObjectMapper.Map<HQTaskDto, HQTaskUpdateDto>(hqDto);
                NavigationManager.NavigateTo($"/Common/HQTasks/edit/{EditingHQTaskId}");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }


        private async Task UpdateHQTaskAsync()
        {
            try
            {
                if (await EditingHQTaskValidations.ValidateAll() == false)
                {
                    return;
                }

                await HQTasksAppService.UpdateAsync(EditingHQTaskId, EditingHQTask);
                var hqDto = await HQTasksAppService.GetAsync(EditingHQTaskId);
                EditingHQTask = ObjectMapper.Map<HQTaskDto, HQTaskUpdateDto>(hqDto);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private void GoToListPage()
        {
            NavigationManager.NavigateTo("/Common/HQTasks");
        }
    }
}
