using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.Common.HQAssigneds;
using HQSOFT.Common.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedRepositoryTests : CommonEntityFrameworkCoreTestBase
    {
        private readonly IHQAssignedRepository _hQAssignedRepository;

        public HQAssignedRepositoryTests()
        {
            _hQAssignedRepository = GetRequiredService<IHQAssignedRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQAssignedRepository.GetListAsync(
                    iDParent: Guid.Parse("0870d8bc-d205-4c2d-b834-fa959cbf95c7"),
                    priority: default,
                    comment: "45dc133bc94b44d08bc102b43d76fb6163ab3bcdc4ab4455ad28a754838b8629d0fbda1c67814571a4d657adcfb641a1"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQAssignedRepository.GetCountAsync(
                    iDParent: Guid.Parse("716a2f30-2eaa-4013-9ae1-6e33437448d1"),
                    priority: default,
                    comment: "2b23ae02ec4343d98c6f9b3c1c9a133667d5604267c04b61b099"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}