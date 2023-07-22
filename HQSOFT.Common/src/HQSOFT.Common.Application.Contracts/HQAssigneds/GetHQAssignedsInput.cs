using HQSOFT.Configuration.HQAssigments;
using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.HQAssigneds
{
    public class GetHQAssignedsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? IDParent { get; set; }
        public DateTime? CompletebyMin { get; set; }
        public DateTime? CompletebyMax { get; set; }
        public PriorityAssign? Priority { get; set; }
        public string? Comment { get; set; }
        public Guid? IdentityUserId { get; set; }

        public GetHQAssignedsInput()
        {

        }
    }
}