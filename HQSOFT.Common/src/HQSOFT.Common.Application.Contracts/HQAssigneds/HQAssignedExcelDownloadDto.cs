using HQSOFT.Configuration.HQAssigments;
using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public Guid? IDParent { get; set; }
        public DateTime? CompletebyMin { get; set; }
        public DateTime? CompletebyMax { get; set; }
        public PriorityAssign? Priority { get; set; }
        public string? Comment { get; set; }
        public Guid? IdentityUserId { get; set; }

        public HQAssignedExcelDownloadDto()
        {

        }
    }
}