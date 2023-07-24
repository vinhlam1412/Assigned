using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.HQShares
{
    public class HQShareCreateDto
    {
        public string? IDParent { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanShare { get; set; }
        public List<Guid> IdentityUserIds { get; set; }
    }
}