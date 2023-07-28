using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public Guid? IDParent { get; set; }
        public Guid? ToUser { get; set; }
        public Guid? FromUser { get; set; }
        public string? NotiTitle { get; set; }
        public string? NotiBody { get; set; }
        public string? Type { get; set; }
        public bool? isRead { get; set; }

        public HQNotificationExcelDownloadDto()
        {

        }
    }
}