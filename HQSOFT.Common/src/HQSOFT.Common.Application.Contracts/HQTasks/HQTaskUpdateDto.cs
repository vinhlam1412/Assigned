using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQTasks
{
    public class HQTaskUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string Subject { get; set; }
        [Required]
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