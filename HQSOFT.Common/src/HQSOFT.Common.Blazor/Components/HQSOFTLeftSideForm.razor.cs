using Blazorise;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQTasks;
using HQSOFT.Common.Shared;
using Microsoft.AspNetCore.Components;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI.Components;
using Volo.Abp.ObjectMapping;
using static HQSOFT.Common.Permissions.CommonPermissions;

namespace HQSOFT.Common.Blazor.Components
{
    public partial class HQSOFTLeftSideForm
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        private IReadOnlyList<HQAssignedWithNavigationPropertiesDto> HQAssignedList { get; set; }
        private IReadOnlyList<LookupDto<Guid>> IdentityUsers { get; set; } = new List<LookupDto<Guid>>();
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private HQAssignedCreateDto NewHQAssigned { get; set; }
        private Validations NewHQAssignedValidations { get; set; } = new();
        private Validations NewHQShareValidations { get; set; } = new();

        private Modal CreateHQShareModal { get; set; } = new();

        private Modal CreateHQAssignedModal { get; set; } = new();

        private HQAssignedUpdateDto EditingHQAssigment { get; set; }

        private Guid EditingHQAssigmentId { get; set; }
        
        private GetHQAssignedsInput Filter { get; set; }

        protected string SelectedCreateTab = "hQAssigment-create-tab";

        protected string SelectedCreateTab2 = "hQShare-create-tab";
        private IReadOnlyList<LookupDto<Guid>> HQTasksCollection { get; set; } = new List<LookupDto<Guid>>();


        public Guid TaskId { get; set; }

        private List<LookupDto<Guid>> SelectedUsers { get; set; } = new List<LookupDto<Guid>>();
        private string SelectedIdentityUserId { get; set; }

        private string SelectedIdentityUserText { get; set; }

        private List<LookupDto<Guid>> SelectedIdentityUsers { get; set; } = new List<LookupDto<Guid>>();

        [Parameter]
        public Guid Value { get; set; }


        //[Parameter]
        //public EventCallback AssignmentClick { get; set; }

        //[Parameter]
        //public Validations Validation { get; set; }
        public HQSOFTLeftSideForm()
        {
            NewHQAssigned = new HQAssignedCreateDto();
            EditingHQAssigment = new HQAssignedUpdateDto();
            Filter = new GetHQAssignedsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            HQAssignedList = new List<HQAssignedWithNavigationPropertiesDto>();
            IdentityUsers =  new List<LookupDto<Guid>>();
        }
        protected override async Task OnInitializedAsync()
        {

            if (Value != Guid.Empty)
            { 
                TaskId = Value;
            }
            if (TaskId != Guid.Empty)
            {
                NewHQAssigned.IDParent = TaskId;
            };
        }
        private async Task GetHQAssigmentsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HQAssignedsAppService.GetListAsync(Filter);
            HQAssignedList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OpenCreateHQAssignedModalAsync()
         {
            //Truyền vào idTask để lấy assingment xem đã tồn tại chưa
            var hQAssigment = await HQAssignedsAppService.GetParentAsync(TaskId);
            
            //Tồn tại
            if (hQAssigment != null)
            {
                //Lấy ra detail của assignment đó: 
                // HQAssigment
                // HQTask 
                // List user
                var hQAssigmentDetail = await HQAssignedsAppService.GetWithNavigationPropertiesAsync(hQAssigment.Id);
            

            //Lấy Id của Assignment
            EditingHQAssigmentId = hQAssigmentDetail.HQAssigned.Id;

            //Lấy Assignment để edit
            EditingHQAssigment = ObjectMapper.Map<HQAssignedDto, HQAssignedUpdateDto>(hQAssigmentDetail.HQAssigned);

                //Map NewHQAssigment từ Update sang Create
                NewHQAssigned = ObjectMapper.Map<HQAssignedUpdateDto, HQAssignedCreateDto>(EditingHQAssigment);
            SelectedUsers = hQAssigmentDetail.IdentityUsers.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.Email}).ToList();
            }

            //Không tồn tại
            else
            {
                //Tạo mới list user và Assigment
                SelectedUsers = new List<LookupDto<Guid>>();

                NewHQAssigned = new HQAssignedCreateDto
                {
                    Completeby = DateTime.Now,
                    


                };
            }
            //await NewHQAssigmentValidations.ClearAll();
            await CreateHQAssignedModal.Show();
        }

        private async Task CloseCreateHQAssignedModalAsync()
        {
            await CreateHQAssignedModal.Hide();
        }

        private async Task CreateHQAssigmentAsync()
        {
            try
            {
                if (await NewHQAssignedValidations.ValidateAll() == false)
                {
                    return;
                }
                NewHQAssigned.IdentityUserIds = SelectedUsers.Select(x => x.Id).ToList();


                if (EditingHQAssigmentId == Guid.Empty)
                {
                    NewHQAssigned.IDParent = TaskId;
                    await HQAssignedsAppService.CreateAsync(NewHQAssigned);
                }
                else
                {
                   await UpdateHQAssigmentAsync();

                }
                await GetHQAssigmentsAsync();
                await CloseCreateHQAssignedModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name; 
            SelectedCreateTab2 = name;
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
        private async Task UpdateHQAssigmentAsync()
        {
            try
            {
                if (await NewHQAssignedValidations.ValidateAll() == false)
                {
                    return;
                }
                EditingHQAssigment.IdentityUserIds = SelectedUsers.Select(x => x.Id).ToList();
                await HQAssignedsAppService.UpdateAsync(EditingHQAssigmentId, EditingHQAssigment);
                await GetHQAssigmentsAsync();
                await CreateHQShareModal.Hide();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        //private void AddUser()
        //{
        //    if (SelectedUserId.IsNullOrEmpty())
        //    {
        //        return;
        //    }

        //    foreach ( var item in SelectedUserId )
        //    {
        //        if (SelectedUsers.Any(p => p.Id.ToString() == item))
        //        {
        //            UiMessageService.Warn(L["ItemAlreadyAdded"]);
        //            return;
        //        }
        //    }

          

        //    for (int i = 0; i < SelectedUserId.Count; i++)
        //    {
        //        Guid userId = Guid.Parse(SelectedUserId[i]);
        //        string userText = SelectedUserText[i];
        //        SelectedUsers.Add(new LookupDto<Guid>
        //        {
        //            Id = userId,
        //            DisplayName = userText
        //        });
        //    }
        //}

    }
}
