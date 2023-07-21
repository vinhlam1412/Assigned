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
                id: Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"),
                iDParent: Guid.Parse("0870d8bc-d205-4c2d-b834-fa959cbf95c7"),
                completeby: new DateTime(2005, 8, 14),
                priority: default,
                comment: "45dc133bc94b44d08bc102b43d76fb6163ab3bcdc4ab4455ad28a754838b8629d0fbda1c67814571a4d657adcfb641a1"
            ));

            await _hQAssignedRepository.InsertAsync(new HQAssigned
            (
                id: Guid.Parse("013e7d8f-568f-4b0f-8516-b20d6b952e8a"),
                iDParent: Guid.Parse("716a2f30-2eaa-4013-9ae1-6e33437448d1"),
                completeby: new DateTime(2015, 1, 13),
                priority: default,
                comment: "2b23ae02ec4343d98c6f9b3c1c9a133667d5604267c04b61b099"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}