using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQTasks
{
    public class HQTaskDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Subject { get; set; }
        public string Project { get; set; }
        public StatusTask Status { get; set; }
        public PriorityTask Priority { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public double ExpectedTime { get; set; }
        public double Process { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}