using Blazorise;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQTasks;
using HQSOFT.Common.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Data;
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
        private IReadOnlyList<HQShareWithNavigationPropertiesDto> HQShareList { get; set; }
        private IReadOnlyList<LookupDto<Guid>> IdentityUsers { get; set; } = new List<LookupDto<Guid>>();
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private HQAssignedCreateDto NewHQAssigned { get; set; }

        private HQAssignedUpdateDto EditingHQAssigned { get; set; }

        private Validations EditingHQAssignedValidations { get; set; } = new();
        private Guid EditingHQAssignedId { get; set; }

        private Modal EditHQAssignedModal { get; set; } = new();

        private GetHQAssignedsInput Filter { get; set; }

        private IReadOnlyList<LookupDto<Guid>> HQTasksCollection { get; set; } = new List<LookupDto<Guid>>();


        public Guid ParentId { get; set; }

        private List<string> SelectedIdentityUserId { get; set; }

        private List<string> SelectedIdentityUserText { get; set; }

        private List<LookupDto<Guid>> SelectedIdentityUsers { get; set; } = new List<LookupDto<Guid>>();


        //-----------------------
        private HQShareCreateDto NewHQShare { get; set; }
        private HQShareUpdateDto EditingHQShare { get; set; }
        private HQShareUpdateDto EditingHQShareAll { get; set; }
        private HQShareUpdateDto EditingHQShareAdd { get; set; }
        private Validations EditingHQShareValidations { get; set; } = new();
        private Guid EditingHQShareId { get; set; }
        private Modal EditHQShareModal { get; set; } = new();
        private GetHQSharesInput FilterShare { get; set; }

        public bool checkvalue { get; set; }


        //-----------------------

        [Parameter]
        public string Value { get; set; }


        //[Parameter]
        //public EventCallback AssignmentClick { get; set; }

        //[Parameter]
        //public Validations Validation { get; set; }
        public HQSOFTLeftSideForm()
        {
            NewHQAssigned = new HQAssignedCreateDto();
            NewHQShare = new HQShareCreateDto();
            EditingHQAssigned = new HQAssignedUpdateDto();
            EditingHQShare = new HQShareUpdateDto();
            EditingHQShareAll = new HQShareUpdateDto();
            EditingHQShareAdd = new HQShareUpdateDto();
            Filter = new GetHQAssignedsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            FilterShare = new GetHQSharesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            HQAssignedList = new List<HQAssignedWithNavigationPropertiesDto>();
            HQShareList = new List<HQShareWithNavigationPropertiesDto>();
            IdentityUsers =  new List<LookupDto<Guid>>();
        }
        protected override async Task OnInitializedAsync()
        {

            if (Value != null && Value.Length > 0)
            {
                ParentId = Guid.Parse(Value);
            }

            //Set IDParent for Assigned from Parent existed
            if (ParentId != Guid.Empty)
            {
                EditingHQAssigned.IDParent = Value;
                EditingHQShare.IDParent = Value;
                EditingHQShareAdd.IDParent = Value;
                var hQAssigment = await HQAssignedsAppService.GetParentAsync(Value);
                if (hQAssigment != null)
                {
                    //Lấy ra detail của assignment đó: 
                    // HQAssigment
                    // HQTask 
                    // List user
                    var hQAssigmentDetail = await HQAssignedsAppService.GetWithNavigationPropertiesAsync(hQAssigment.Id);                
                    SelectedIdentityUsers = hQAssigmentDetail.IdentityUsers.Select(a => new LookupDto<Guid> { Id = a.Id, DisplayName = a.Email }).ToList();
                }
            };
            SelectedIdentityUserId = new List<string>();
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

        private async Task GetHQSharesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HQSharesAppService.GetListAsync(FilterShare);
            HQShareList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OpenEditHQAssignedModalAsync()
         {
            //Truyền vào ParentID để lấy assingment xem đã tồn tại chưa
            var hQAssigment = await HQAssignedsAppService.GetParentAsync(Value);
            
            //Tồn tại
            if (hQAssigment != null)
            {
                //Lấy ra detail của assignment đó: 
                // HQAssigment
                // HQTask 
                // List user
                var hQAssigmentDetail = await HQAssignedsAppService.GetWithNavigationPropertiesAsync(hQAssigment.Id);


                //Lấy Id của Assignment
                EditingHQAssignedId = hQAssigmentDetail.HQAssigned.Id;

                //Lấy Assignment để edit
                EditingHQAssigned = ObjectMapper.Map<HQAssignedDto, HQAssignedUpdateDto>(hQAssigmentDetail.HQAssigned);

                //Map NewHQAssigment từ Update sang Create
                NewHQAssigned = ObjectMapper.Map<HQAssignedUpdateDto, HQAssignedCreateDto>(EditingHQAssigned);
                SelectedIdentityUsers = hQAssigmentDetail.IdentityUsers.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.Email}).ToList();
            }

            //Không tồn tại
            else
            {
                //Tạo mới list user và Assigment
                SelectedIdentityUsers = new List<LookupDto<Guid>>();

                NewHQAssigned = new HQAssignedCreateDto
                {
                    Completeby = DateTime.Now,
                    


                };
            }
            //await NewHQAssigmentValidations.ClearAll();
            await EditHQAssignedModal.Show();
        }

        private async Task OpenEditHQShareModalAsync()
        {
            var hQShare = await HQSharesAppService.GetParentAsync(Value);

            if(hQShare != null)
            {
                var hQShareDetail = await HQSharesAppService.GetWithNavigationPropertiesAsync(hQShare.Id);

                EditingHQShareId = hQShareDetail.HQShare.Id;
                EditingHQShare = ObjectMapper.Map<HQShareDto, HQShareUpdateDto>(hQShareDetail.HQShare);

                NewHQShare = ObjectMapper.Map<HQShareUpdateDto,HQShareCreateDto>(EditingHQShare);
                SelectedIdentityUsers = hQShareDetail.IdentityUsers.Select(a => new LookupDto<Guid> { Id = a.Id, DisplayName = a.Email }).ToList();
            }
            else
            {
                //Tạo mới list user và Assigment
                SelectedIdentityUsers = new List<LookupDto<Guid>>();

                EditingHQShare = new HQShareUpdateDto
                {
                    
                };
            }
            await EditingHQShareValidations.ClearAll();
            await EditHQShareModal.Show();
        }

        private async Task CloseCreateHQAssignedModalAsync()
        {
            await EditHQAssignedModal.Hide();
        }

        private async Task CloseEditHQShareModalAsync()
        {
            await EditHQShareModal.Hide();
        }

        private async Task CreateHQAssigmentAsync()
        {
            try
            {
                if (await EditingHQAssignedValidations.ValidateAll() == false)
                {
                    return;
                }
                //EditingHQAssigned.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();


                //Create new Assigned if Parent dont have Assigned
                if (EditingHQAssignedId == Guid.Empty)
                {
                    NewHQAssigned = ObjectMapper.Map<HQAssignedUpdateDto,HQAssignedCreateDto>(EditingHQAssigned);
                    AddIdentityUser();
                    NewHQAssigned.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();
                    EditingHQAssigned.IDParent = Value;
                    await HQAssignedsAppService.CreateAsync(NewHQAssigned);
                }

                //Update Assigned if Parent have Assigned
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

        private async Task CreateHQShareAsync()
        {
            try
            {
                if (await EditingHQShareValidations.ValidateAll() == false)
                {
                    return;
                }
                NewHQShare.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();

                if(EditingHQShareId == Guid.Empty)
                {
                    NewHQShare = ObjectMapper.Map<HQShareUpdateDto, HQShareCreateDto>(EditingHQShareAdd);
                    NewHQShare.CanRead = true;
                    NewHQShare.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();
                    NewHQShare.IDParent = Value;
                    await HQSharesAppService.CreateAsync(NewHQShare);
                }
                else
                {
                    await UpdateHQShareAsync();

                }

                await GetHQSharesAsync();
                await CloseEditHQShareModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task 
            GetIdentityUserLookupAsync(string? newValue = null)
        {
            IdentityUsers = (await HQAssignedsAppService.GetIdentityUserLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

     
        private async Task UpdateHQAssigmentAsync()
        {
            try
            {
                if (await EditingHQAssignedValidations.ValidateAll() == false)
                {
                    return;
                }
                AddIdentityUser();
                EditingHQAssigned.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();
                await HQAssignedsAppService.UpdateAsync(EditingHQAssignedId, EditingHQAssigned);
                await GetHQAssigmentsAsync();
                await EditHQAssignedModal.Hide();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task UpdateHQShareAsync()
        {
            try
            {
                if (await EditingHQShareValidations.ValidateAll() == false)
                {
                    return;
                }
                EditingHQShare.IdentityUserIds = SelectedIdentityUsers.Select(x => x.Id).ToList();
                await HQSharesAppService.UpdateAsync(EditingHQShareId, EditingHQShare);
                await GetHQSharesAsync();
                await EditHQShareModal.Hide();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void AddIdentityUser()
        {
            if (SelectedIdentityUserId.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in SelectedIdentityUserId)
            {
                if (SelectedIdentityUsers.Any(p => p.Id.ToString() == item))
                {
                    UiMessageService.Warn(L["ItemAlreadyAdded"]);
                    return;
                }
            }


            for (int i = 0; i < SelectedIdentityUserId.Count; i++)
            {
                Guid userId = Guid.Parse(SelectedIdentityUserId[i]);
                string userText = SelectedIdentityUserText[i];
                SelectedIdentityUsers.Add(new LookupDto<Guid>
                {
                    Id = userId,
                    DisplayName = userText
                });
            }
        }

    }
}
