using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.Common.HQAssigneds;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IHQAssignedRepository _hQAssignedRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public HQAssignedsDataSeedContributor(IHQAssignedRepository hQAssignedRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _hQAssignedRepository = hQAssignedRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _hQAssignedRepository.InsertAsync(new HQAssigned
            (
                id: Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"),
                iDParent: "6f85f39773",
                completeby: new DateTime(2013, 1, 24),
                priority: default,
                comment: "61c050ef23e04c838cf9076688f6a"
            ));

            await _hQAssignedRepository.InsertAsync(new HQAssigned
            (
                id: Guid.Parse("dad45d4d-5471-4b58-b233-6044f271a07c"),
                iDParent: "f2e5c07619474254b9e08",
                completeby: new DateTime(2013, 2, 2),
                priority: default,
                comment: "2840f601bf3a457287571823223621608de445764bd240f6a86b05edbcd9effc25363b435116425a8f800"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}