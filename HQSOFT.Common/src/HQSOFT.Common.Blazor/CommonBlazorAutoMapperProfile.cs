using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.HQAssigneds;
using Volo.Abp.AutoMapper;
using HQSOFT.Common.HQTasks;
using AutoMapper;
using Volo.Abp.Identity;

namespace HQSOFT.Common.Blazor;

public class CommonBlazorAutoMapperProfile : Profile
{
    public CommonBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<HQTaskDto, HQTaskUpdateDto>();
        CreateMap<HQTaskUpdateDto, HQTaskCreateDto>();

        CreateMap<HQAssignedDto, HQAssignedUpdateDto>().Ignore(x => x.IdentityUserIds);
        CreateMap<HQAssignedUpdateDto, HQAssignedCreateDto>().Ignore(x => x.IdentityUserIds);

        CreateMap<HQShareDto, HQShareUpdateDto>();
        CreateMap<HQShareUpdateDto, HQShareCreateDto>();

        CreateMap<HQNotificationDto, HQNotificationUpdateDto>();
        CreateMap<HQNotificationDto, HQNotificationCreateDto>();
    }
}