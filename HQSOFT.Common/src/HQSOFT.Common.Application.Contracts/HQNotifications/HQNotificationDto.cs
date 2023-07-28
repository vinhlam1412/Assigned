using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid IDParent { get; set; }
        public Guid ToUser { get; set; }
        public Guid FromUser { get; set; }
        public string? NotiTitle { get; set; }
        public string? NotiBody { get; set; }
        public string? Type { get; set; }
        public bool isRead { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}