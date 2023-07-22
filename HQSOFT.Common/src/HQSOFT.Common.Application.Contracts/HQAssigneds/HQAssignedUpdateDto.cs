using HQSOFT.Configuration.HQAssigments;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedUpdateDto : IHasConcurrencyStamp
    {
        public string? IDParent { get; set; }
        public DateTime Completeby { get; set; }
        public PriorityAssign Priority { get; set; }
        public string? Comment { get; set; }
        public List<Guid> IdentityUserIds { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}