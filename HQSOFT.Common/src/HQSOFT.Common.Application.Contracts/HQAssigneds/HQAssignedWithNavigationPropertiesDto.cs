using Volo.Abp.Identity;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedWithNavigationPropertiesDto
    {
        public HQAssignedDto HQAssigned { get; set; }

        public List<IdentityUserDto> IdentityUsers { get; set; }

    }
}