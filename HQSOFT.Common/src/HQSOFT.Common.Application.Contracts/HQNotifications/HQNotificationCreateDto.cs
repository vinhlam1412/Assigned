using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationCreateDto
    {
        public Guid IDParent { get; set; }
        public Guid ToUser { get; set; }
        public Guid FromUser { get; set; }
        public string? NotiTitle { get; set; }
        public string? NotiBody { get; set; }
        public string? Type { get; set; }
        public bool isRead { get; set; }
    }
}