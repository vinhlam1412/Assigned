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
                id: Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"),
                iDParent: "a7952fe98f8645f3856ce6da3e84bc03bd10df7ba2bd4f5c9ce4d0782",
                canRead: true,
                canWrite: true,
                canSubmit: true,
                canShare: true,
                identityUserId: null
            ));

            await _hQShareRepository.InsertAsync(new HQShare
            (
                id: Guid.Parse("8a14eeb2-4efd-426f-80f1-ab64826e6de2"),
                iDParent: "1609b18",
                canRead: true,
                canWrite: true,
                canSubmit: true,
                canShare: true,
                identityUserId: null
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}