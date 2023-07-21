using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.Common.HQTasks;
using HQSOFT.Common.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.Common.HQTasks
{
    public class HQTaskRepositoryTests : CommonEntityFrameworkCoreTestBase
    {
        private readonly IHQTaskRepository _hQTaskRepository;

        public HQTaskRepositoryTests()
        {
            _hQTaskRepository = GetRequiredService<IHQTaskRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQTaskRepository.GetListAsync(
                    subject: "d7ed793eb32946fca3c3f9104fdedffd935a1401b12c4b45889a0e389",
                    project: "d7d52b45c4b7427e9419f5293b1f4f764d3ea7fc12304529baae82242fbf48",
                    status: default,
                    priority: default
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQTaskRepository.GetCountAsync(
                    subject: "aaf9dac38b424a7db90b33648c460c3434e658aaf4204d",
                    project: "b04ac08d7aea4996ad705e0e7cb72ad41ee932af543b43cfbdf49d68ed411bab53cd5c",
                    status: default,
                    priority: default
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}