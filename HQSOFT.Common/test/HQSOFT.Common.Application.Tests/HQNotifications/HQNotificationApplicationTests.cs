using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.Common.HQNotifications
{
    public class HQNotificationsAppServiceTests : CommonApplicationTestBase
    {
        private readonly IHQNotificationsAppService _hQNotificationsAppService;
        private readonly IRepository<HQNotification, Guid> _hQNotificationRepository;

        public HQNotificationsAppServiceTests()
        {
            _hQNotificationsAppService = GetRequiredService<IHQNotificationsAppService>();
            _hQNotificationRepository = GetRequiredService<IRepository<HQNotification, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _hQNotificationsAppService.GetListAsync(new GetHQNotificationsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("2b10b34d-e573-4190-b73c-af443591fed6")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hQNotificationsAppService.GetAsync(Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HQNotificationCreateDto
            {
                IDParent = Guid.Parse("8d1078f0-c527-4816-82af-abfda0fbaa99"),
                ToUser = Guid.Parse("0736735b-5710-40b7-b2b9-5a4fa6769b6a"),
                FromUser = Guid.Parse("fa0efdfa-5254-4294-8182-891370ba7650"),
                NotiTitle = "445da65d4ccc45ec8f41fa6235e70ca2faef106a025144",
                NotiBody = "77166698f1254486b0e00ed385d7c575e33b",
                Type = "14635d97df544449a26fa6319fe96073097d3181249148f2ab6e8a3522ff6a940c435691659b420c95b66",
                isRead = true
            };

            // Act
            var serviceResult = await _hQNotificationsAppService.CreateAsync(input);

            // Assert
            var result = await _hQNotificationRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe(Guid.Parse("8d1078f0-c527-4816-82af-abfda0fbaa99"));
            result.ToUser.ShouldBe(Guid.Parse("0736735b-5710-40b7-b2b9-5a4fa6769b6a"));
            result.FromUser.ShouldBe(Guid.Parse("fa0efdfa-5254-4294-8182-891370ba7650"));
            result.NotiTitle.ShouldBe("445da65d4ccc45ec8f41fa6235e70ca2faef106a025144");
            result.NotiBody.ShouldBe("77166698f1254486b0e00ed385d7c575e33b");
            result.Type.ShouldBe("14635d97df544449a26fa6319fe96073097d3181249148f2ab6e8a3522ff6a940c435691659b420c95b66");
            result.isRead.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HQNotificationUpdateDto()
            {
                IDParent = Guid.Parse("4dd73f34-db25-4bf8-91bc-849dea6bee10"),
                ToUser = Guid.Parse("7762a8aa-fd6a-497a-872f-a51f55c98cd5"),
                FromUser = Guid.Parse("41d7a032-7244-441c-99db-135135592cbc"),
                NotiTitle = "2c5602adefcc43",
                NotiBody = "cfe1ca6fb182462",
                Type = "a9a00244a63b41f4818e8",
                isRead = true
            };

            // Act
            var serviceResult = await _hQNotificationsAppService.UpdateAsync(Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"), input);

            // Assert
            var result = await _hQNotificationRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe(Guid.Parse("4dd73f34-db25-4bf8-91bc-849dea6bee10"));
            result.ToUser.ShouldBe(Guid.Parse("7762a8aa-fd6a-497a-872f-a51f55c98cd5"));
            result.FromUser.ShouldBe(Guid.Parse("41d7a032-7244-441c-99db-135135592cbc"));
            result.NotiTitle.ShouldBe("2c5602adefcc43");
            result.NotiBody.ShouldBe("cfe1ca6fb182462");
            result.Type.ShouldBe("a9a00244a63b41f4818e8");
            result.isRead.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hQNotificationsAppService.DeleteAsync(Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"));

            // Assert
            var result = await _hQNotificationRepository.FindAsync(c => c.Id == Guid.Parse("8589fc06-bb8f-4760-b5ce-600d045f5e4d"));

            result.ShouldBeNull();
        }
    }
}