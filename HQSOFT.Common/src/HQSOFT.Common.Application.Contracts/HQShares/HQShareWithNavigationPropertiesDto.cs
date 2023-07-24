using Volo.Abp.Identity;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace HQSOFT.Common.HQShares
{
    public class HQShareWithNavigationPropertiesDto
    {
        public HQShareDto HQShare { get; set; }

        public List<IdentityUserDto> IdentityUsers { get; set; }

    }
}