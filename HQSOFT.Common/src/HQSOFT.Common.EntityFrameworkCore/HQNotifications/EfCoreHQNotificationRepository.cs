using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.Common.EntityFrameworkCore;

namespace HQSOFT.Common.HQNotifications
{
    public class EfCoreHQNotificationRepository : EfCoreRepository<CommonDbContext, HQNotification, Guid>, IHQNotificationRepository
    {
        public EfCoreHQNotificationRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<HQNotification>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, iDParent, toUser, fromUser, notiTitle, notiBody, type, isRead);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQNotificationConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            Guid? iDParent = null,
            Guid? toUser = null,
            Guid? fromUser = null,
            string notiTitle = null,
            string notiBody = null,
            string type = null,
            bool? isRead = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, iDParent, toUser, fromUser, notiTitle, notiBody, type, isRead);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<HQNotification> ApplyFilter(
            IQueryable<HQNotification> query,
            string filterText,
            Guid? iDParent = null,
            Guid? toUser = null,
            Guid? fromUser = null,
            string notiTitle = null,
            string notiBody = null,
            string type = null,
            bool? isRead = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.NotiTitle.Contains(filterText) || e.NotiBody.Contains(filterText) || e.Type.Contains(filterText))
                    .WhereIf(iDParent.HasValue, e => e.IDParent == iDParent)
                    .WhereIf(toUser.HasValue, e => e.ToUser == toUser)
                    .WhereIf(fromUser.HasValue, e => e.FromUser == fromUser)
                    .WhereIf(!string.IsNullOrWhiteSpace(notiTitle), e => e.NotiTitle.Contains(notiTitle))
                    .WhereIf(!string.IsNullOrWhiteSpace(notiBody), e => e.NotiBody.Contains(notiBody))
                    .WhereIf(!string.IsNullOrWhiteSpace(type), e => e.Type.Contains(type))
                    .WhereIf(isRead.HasValue, e => e.isRead == isRead);
        }
    }
}