using HQSOFT.Configuration.HQAssigments;
using System;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedExcelDto
    {
        public Guid IDParent { get; set; }
        public DateTime Completeby { get; set; }
        public PriorityAssign Priority { get; set; }
        public string? Comment { get; set; }
    }
}