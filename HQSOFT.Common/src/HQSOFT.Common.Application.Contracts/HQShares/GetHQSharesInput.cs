using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.HQShares
{
    public class GetHQSharesInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? IDParent { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanWrite { get; set; }
        public bool? CanSubmit { get; set; }
        public bool? CanShare { get; set; }
        public Guid? IdentityUserId { get; set; }

        public GetHQSharesInput()
        {

        }
    }
}