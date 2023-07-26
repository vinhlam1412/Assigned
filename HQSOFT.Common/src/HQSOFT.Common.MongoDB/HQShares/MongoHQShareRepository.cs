using Volo.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using HQSOFT.Common.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace HQSOFT.Common.HQShares
{
    public class MongoHQShareRepository : MongoDbRepository<CommonMongoDbContext, HQShare, Guid>, IHQShareRepository
    {
        public MongoHQShareRepository(IMongoDbContextProvider<CommonMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<HQShareWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var hQShare = await (await GetMongoQueryableAsync(cancellationToken))
                .FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken));

            var identityUser = await (await GetDbContextAsync(cancellationToken)).Database.GetCollection<IdentityUser>("AbpUsers").AsQueryable().FirstOrDefaultAsync(e => e.Id == hQShare.IdentityUserId, cancellationToken: cancellationToken);

            return new HQShareWithNavigationProperties
            {
                HQShare = hQShare,
                IdentityUser = identityUser,

            };
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, iDParent, canRead, canWrite, canSubmit, canShare, identityUserId);
            var hQShares = await query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQShareConsts.GetDefaultSorting(false) : sorting.Split('.').Last())
                .As<IMongoQueryable<HQShare>>()
                .PageBy<HQShare, IMongoQueryable<HQShare>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            var dbContext = await GetDbContextAsync(cancellationToken);
            return hQShares.Select(s => new HQShareWithNavigationProperties
            {
                HQShare = s,
                IdentityUser = dbContext.Database.GetCollection<IdentityUser>("AbpUsers").AsQueryable().FirstOrDefault(e => e.Id == s.IdentityUserId),

            }).ToList();
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, iDParent, canRead, canWrite, canSubmit, canShare);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HQShareConsts.GetDefaultSorting(false) : sorting);
            return await query.As<IMongoQueryable<HQShare>>()
                .PageBy<HQShare, IMongoQueryable<HQShare>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
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
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, iDParent, canRead, canWrite, canSubmit, canShare, identityUserId);
            return await query.As<IMongoQueryable<HQShare>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<HQShare> ApplyFilter(
            IQueryable<HQShare> query,
            string filterText,
            string iDParent = null,
            bool? canRead = null,
            bool? canWrite = null,
            bool? canSubmit = null,
            bool? canShare = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.IDParent.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(iDParent), e => e.IDParent.Contains(iDParent))
                    .WhereIf(canRead.HasValue, e => e.CanRead == canRead)
                    .WhereIf(canWrite.HasValue, e => e.CanWrite == canWrite)
                    .WhereIf(canSubmit.HasValue, e => e.CanSubmit == canSubmit)
                    .WhereIf(canShare.HasValue, e => e.CanShare == canShare)
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUserId == identityUserId);
        }
    }
}