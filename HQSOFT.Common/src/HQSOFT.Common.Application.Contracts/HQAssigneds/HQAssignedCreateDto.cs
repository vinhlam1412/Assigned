using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedCreateDto
    {
        public Guid IDParent { get; set; }
        public DateTime Completeby { get; set; }
        public PriorityAssign Priority { get; set; } = ((PriorityAssign[])Enum.GetValues(typeof(PriorityAssign)))[0];
        public string? Comment { get; set; }
        public List<Guid> IdentityUserIds { get; set; }
    }
}