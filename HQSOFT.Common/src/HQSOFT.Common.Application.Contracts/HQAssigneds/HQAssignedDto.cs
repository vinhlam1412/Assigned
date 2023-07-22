using HQSOFT.Configuration.HQAssigments;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? IDParent { get; set; }
        public DateTime Completeby { get; set; }
        public PriorityAssign Priority { get; set; }
        public string? Comment { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}