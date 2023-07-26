using Volo.Abp.Identity;
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

namespace HQSOFT.Common.HQShares
{
    public class EfCoreHQShareRepository : EfCoreRepository<CommonDbContext, HQShare, Guid>, IHQShareRepository
    {
        public EfCoreHQShareRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<HQShareWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(hQShare => new HQShareWithNavigationProperties
                {
                    HQShare = hQShare,
                    IdentityUser = dbContext.Set<IdentityUser>().FirstOrDefault(c => c.Id == hQShare.IdentityUserId)
                }).FirstOrDefault();
        }

        public async Task<List<HQShareWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            Guid? identityUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, iDParent, canRead, canWrite, canSubmit, canShare, identityUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQShareConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<HQShareWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from hQShare in (await GetDbSetAsync())
                   join identityUser in (await GetDbContextAsync()).Set<IdentityUser>() on hQShare.IdentityUserId equals identityUser.Id into users
                   from identityUser in users.DefaultIfEmpty()
                   select new HQShareWithNavigationProperties
                   {
                       HQShare = hQShare,
                       IdentityUser = identityUser
                   };
        }

        protected virtual IQueryable<HQShareWithNavigationProperties> ApplyFilter(
            IQueryable<HQShareWithNavigationProperties> query,
            string filterText,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.HQShare.IDParent.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(iDParent), e => e.HQShare.IDParent.Contains(iDParent))
                    .WhereIf(canRead.HasValue, e => e.HQShare.CanRead == canRead)
                    .WhereIf(canWrite.HasValue, e => e.HQShare.CanWrite == canWrite)
                    .WhereIf(canSubmit.HasValue, e => e.HQShare.CanSubmit == canSubmit)
                    .WhereIf(canShare.HasValue, e => e.HQShare.CanShare == canShare)
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUser != null && e.IdentityUser.Id == identityUserId);
        }

        public async Task<List<HQShare>> GetListAsync(
            string filterText = null,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, iDParent, canRead, canWrite, canSubmit, canShare);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQShareConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, iDParent, canRead, canWrite, canSubmit, canShare, identityUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<HQShare> ApplyFilter(
            IQueryable<HQShare> query,
            string filterText,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.IDParent.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(iDParent), e => e.IDParent.Contains(iDParent))
                    .WhereIf(canRead.HasValue, e => e.CanRead == canRead)
                    .WhereIf(canWrite.HasValue, e => e.CanWrite == canWrite)
                    .WhereIf(canSubmit.HasValue, e => e.CanSubmit == canSubmit)
                    .WhereIf(canShare.HasValue, e => e.CanShare == canShare);
        }
    }
}