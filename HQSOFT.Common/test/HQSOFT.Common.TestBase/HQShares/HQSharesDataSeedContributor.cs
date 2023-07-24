using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.Common.HQShares;

namespace HQSOFT.Common.HQShares
{
    public class HQSharesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IHQShareRepository _hQShareRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public HQSharesDataSeedContributor(IHQShareRepository hQShareRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _hQShareRepository = hQShareRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _hQShareRepository.InsertAsync(new HQShare
            (
                id: Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"),
                iDParent: "a54d307ca4544bc68fc6e9b5d70b651bcd34",
                canRead: true,
                canWrite: true,
                canSubmit: true,
                canShare: true
            ));

            await _hQShareRepository.InsertAsync(new HQShare
            (
                id: Guid.Parse("1813abc1-7745-4754-83cd-154677e5c689"),
                iDParent: "2552b6bb0625446c8c3e129d20aba84d443c5984c6974e85aabefd3bf4eb767eda5ae011380b4e76a57c07c91e5e",
                canRead: true,
                canWrite: true,
                canSubmit: true,
                canShare: true
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}