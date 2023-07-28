using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.Common.HQNotifications;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IHQNotificationRepository _hQNotificationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public HQNotificationsDataSeedContributor(IHQNotificationRepository hQNotificationRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _hQNotificationRepository = hQNotificationRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _hQNotificationRepository.InsertAsync(new HQNotification
            (
                id: Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"),
                iDParent: Guid.Parse("f39379cb-d441-42ef-81a9-ea21e3ad0f51"),
                toUser: Guid.Parse("1e7ab837-be0c-44fe-8331-a4f0a2633a73"),
                fromUser: Guid.Parse("629cffb7-2064-4ca9-a930-a992d43e482f"),
                notiTitle: "d8c2b73404cf47309a9af2d5d9200d9cb27fdfb8ea6647d497d1e2608d365e4bbcfa",
                notiBody: "880592a7af3b4d1f92ea2cd",
                type: "639e1daed58b4b9c942c63e9a37e426a8e9cffbc855c4f2489ec0536ef03f7dee8b8e5363fe640d9b5a2cb0b86",
                isRead: true
            ));

            await _hQNotificationRepository.InsertAsync(new HQNotification
            (
                id: Guid.Parse("2b10b34d-e573-4190-b73c-af443591fed6"),
                iDParent: Guid.Parse("c5fb3f35-768f-496d-8fe0-3466713f4e98"),
                toUser: Guid.Parse("24118d77-1aaf-4271-898c-b7df300dab7c"),
                fromUser: Guid.Parse("dfd6c74e-1cab-4013-8dcf-fcceec15a0d5"),
                notiTitle: "a5736eb668e84a76b71f8",
                notiBody: "df2290921c594d1f83b5f1bce270011392ca052a0f5c4e8ea937ed5166861b3c434e4d4c43",
                type: "d27d59a85514455cb6e21091932f5892b70",
                isRead: true
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}