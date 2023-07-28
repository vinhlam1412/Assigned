using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotification : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid IDParent { get; set; }

        public virtual Guid ToUser { get; set; }

        public virtual Guid FromUser { get; set; }

        [CanBeNull]
        public virtual string? NotiTitle { get; set; }

        [CanBeNull]
        public virtual string? NotiBody { get; set; }

        [CanBeNull]
        public virtual string? Type { get; set; }

        public virtual bool isRead { get; set; }

        public HQNotification()
        {

        }

        public HQNotification(Guid id, Guid iDParent, Guid toUser, Guid fromUser, string notiTitle, string notiBody, string type, bool isRead)
        {

            Id = id;
            IDParent = iDParent;
            ToUser = toUser;
            FromUser = fromUser;
            NotiTitle = notiTitle;
            NotiBody = notiBody;
            Type = type;
            this.isRead = isRead;
        }

    }
}