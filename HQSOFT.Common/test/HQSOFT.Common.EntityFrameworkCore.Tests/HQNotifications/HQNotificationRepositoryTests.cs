using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.Common.HQNotifications;
using HQSOFT.Common.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationRepositoryTests : CommonEntityFrameworkCoreTestBase
    {
        private readonly IHQNotificationRepository _hQNotificationRepository;

        public HQNotificationRepositoryTests()
        {
            _hQNotificationRepository = GetRequiredService<IHQNotificationRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQNotificationRepository.GetListAsync(
                    iDParent: Guid.Parse("f39379cb-d441-42ef-81a9-ea21e3ad0f51"),
                    toUser: Guid.Parse("1e7ab837-be0c-44fe-8331-a4f0a2633a73"),
                    fromUser: Guid.Parse("629cffb7-2064-4ca9-a930-a992d43e482f"),
                    notiTitle: "d8c2b73404cf47309a9af2d5d9200d9cb27fdfb8ea6647d497d1e2608d365e4bbcfa",
                    notiBody: "880592a7af3b4d1f92ea2cd",
                    type: "639e1daed58b4b9c942c63e9a37e426a8e9cffbc855c4f2489ec0536ef03f7dee8b8e5363fe640d9b5a2cb0b86",
                    isRead: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQNotificationRepository.GetCountAsync(
                    iDParent: Guid.Parse("c5fb3f35-768f-496d-8fe0-3466713f4e98"),
                    toUser: Guid.Parse("24118d77-1aaf-4271-898c-b7df300dab7c"),
                    fromUser: Guid.Parse("dfd6c74e-1cab-4013-8dcf-fcceec15a0d5"),
                    notiTitle: "a5736eb668e84a76b71f8",
                    notiBody: "df2290921c594d1f83b5f1bce270011392ca052a0f5c4e8ea937ed5166861b3c434e4d4c43",
                    type: "d27d59a85514455cb6e21091932f5892b70",
                    isRead: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}