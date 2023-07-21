using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.Common.HQTasks;

namespace HQSOFT.Common.HQTasks
{
    public class HQTasksDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IHQTaskRepository _hQTaskRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public HQTasksDataSeedContributor(IHQTaskRepository hQTaskRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _hQTaskRepository = hQTaskRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _hQTaskRepository.InsertAsync(new HQTask
            (
                id: Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"),
                subject: "d7ed793eb32946fca3c3f9104fdedffd935a1401b12c4b45889a0e389",
                project: "d7d52b45c4b7427e9419f5293b1f4f764d3ea7fc12304529baae82242fbf48",
                status: default,
                priority: default,
                expectedStartDate: new DateTime(2012, 7, 7),
                expectedEndDate: new DateTime(2017, 8, 6),
                expectedTime: 162147020,
                process: 175350679
            ));

            await _hQTaskRepository.InsertAsync(new HQTask
            (
                id: Guid.Parse("15b45a77-923c-4a36-9be3-4e900edfac50"),
                subject: "aaf9dac38b424a7db90b33648c460c3434e658aaf4204d",
                project: "b04ac08d7aea4996ad705e0e7cb72ad41ee932af543b43cfbdf49d68ed411bab53cd5c",
                status: default,
                priority: default,
                expectedStartDate: new DateTime(2006, 9, 7),
                expectedEndDate: new DateTime(2006, 3, 20),
                expectedTime: 1250363447,
                process: 1196438881
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}