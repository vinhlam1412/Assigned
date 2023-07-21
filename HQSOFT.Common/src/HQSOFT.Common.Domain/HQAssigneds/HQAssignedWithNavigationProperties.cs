using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedWithNavigationProperties
    {
        public HQAssigned HQAssigned { get; set; }

        

        public List<IdentityUser> IdentityUsers { get; set; }
        
    }
}