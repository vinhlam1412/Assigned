using HQSOFT.Common.Web.Pages.Common.HQNotifications;
using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.Web.Pages.Common.HQShares;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.Web.Pages.Common.HQAssigneds;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.Web.Pages.Common.HQTasks;
using Volo.Abp.AutoMapper;
using HQSOFT.Common.HQTasks;
using AutoMapper;

namespace HQSOFT.Common.Web;

public class CommonWebAutoMapperProfile : Profile
{
    public CommonWebAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<HQTaskDto, HQTaskUpdateViewModel>();
        CreateMap<HQTaskUpdateViewModel, HQTaskUpdateDto>();
        CreateMap<HQTaskCreateViewModel, HQTaskCreateDto>();

        CreateMap<HQAssignedDto, HQAssignedUpdateViewModel>();
        CreateMap<HQAssignedUpdateViewModel, HQAssignedUpdateDto>();
        CreateMap<HQAssignedCreateViewModel, HQAssignedCreateDto>();

        CreateMap<HQAssignedDto, HQAssignedUpdateViewModel>().Ignore(x => x.IdentityUserIds);
        CreateMap<HQAssignedUpdateViewModel, HQAssignedUpdateDto>();
        CreateMap<HQAssignedCreateViewModel, HQAssignedCreateDto>();

        CreateMap<HQShareDto, HQShareUpdateViewModel>();
        CreateMap<HQShareUpdateViewModel, HQShareUpdateDto>();
        CreateMap<HQShareCreateViewModel, HQShareCreateDto>();

        CreateMap<HQNotificationDto, HQNotificationUpdateViewModel>();
        CreateMap<HQNotificationUpdateViewModel, HQNotificationUpdateDto>();
        CreateMap<HQNotificationCreateViewModel, HQNotificationCreateDto>();
    }
}