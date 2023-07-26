using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace HQSOFT.Common.HQShares
{
    public class HQShareWithNavigationProperties
    {
        public HQShare HQShare { get; set; }

        public IdentityUser IdentityUser { get; set; }
        

        
    }
}