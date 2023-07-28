using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.Common.HQNotifications
{
    public interface IHQNotificationRepository : IRepository<HQNotification, Guid>
    {
        Task<List<HQNotification>> GetListAsync(
            string filterText = null,
            Guid? iDParent = null,
            Guid? toUser = null,
            Guid? fromUser = null,
            string notiTitle = null,
            string notiBody = null,
            string type = null,
            bool? isRead = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            Guid? iDParent = null,
            Guid? toUser = null,
            Guid? fromUser = null,
            string notiTitle = null,
            string notiBody = null,
            string type = null,
            bool? isRead = null,
            CancellationToken cancellationToken = default);
    }
}