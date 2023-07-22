using Volo.Abp.Identity;
using HQSOFT.Common.HQAssigneds;
using System;
using HQSOFT.Common.Shared;
using Volo.Abp.AutoMapper;
using HQSOFT.Common.HQTasks;
using AutoMapper;

namespace HQSOFT.Common;

public class CommonApplicationAutoMapperProfile : Profile
{
    public CommonApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<HQTask, HQTaskDto>();
        CreateMap<HQTask, HQTaskExcelDto>();

        CreateMap<HQAssigned, HQAssignedDto>();
        CreateMap<HQAssigned, HQAssignedExcelDto>();

        CreateMap<HQAssignedWithNavigationProperties, HQAssignedWithNavigationPropertiesDto>();
        CreateMap<IdentityUser, IdentityUserDto>();
        CreateMap<IdentityUser, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Email));
        CreateMap<IdentityUserDto, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Email));
    }
}