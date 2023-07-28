using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationManager : DomainService
    {
        private readonly IHQNotificationRepository _hQNotificationRepository;

        public HQNotificationManager(IHQNotificationRepository hQNotificationRepository)
        {
            _hQNotificationRepository = hQNotificationRepository;
        }

        public async Task<HQNotification> CreateAsync(
        Guid iDParent, Guid toUser, Guid fromUser, string notiTitle, string notiBody, string type, bool isRead)
        {

            var hQNotification = new HQNotification(
             GuidGenerator.Create(),
             iDParent, toUser, fromUser, notiTitle, notiBody, type, isRead
             );

            return await _hQNotificationRepository.InsertAsync(hQNotification);
        }

        public async Task<HQNotification> UpdateAsync(
            Guid id,
            Guid iDParent, Guid toUser, Guid fromUser, string notiTitle, string notiBody, string type, bool isRead, [CanBeNull] string concurrencyStamp = null
        )
        {

            var hQNotification = await _hQNotificationRepository.GetAsync(id);

            hQNotification.IDParent = iDParent;
            hQNotification.ToUser = toUser;
            hQNotification.FromUser = fromUser;
            hQNotification.NotiTitle = notiTitle;
            hQNotification.NotiBody = notiBody;
            hQNotification.Type = type;
            hQNotification.isRead = isRead;

            hQNotification.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hQNotificationRepository.UpdateAsync(hQNotification);
        }

    }
}