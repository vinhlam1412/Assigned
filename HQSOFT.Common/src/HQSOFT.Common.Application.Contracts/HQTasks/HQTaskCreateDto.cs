using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.HQTasks
{
    public class HQTaskCreateDto
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Project { get; set; }
        public StatusTask Status { get; set; } = ((StatusTask[])Enum.GetValues(typeof(StatusTask)))[0];
        public PriorityTask Priority { get; set; } = ((PriorityTask[])Enum.GetValues(typeof(PriorityTask)))[0];
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public double ExpectedTime { get; set; }
        public double Process { get; set; }
    }
}