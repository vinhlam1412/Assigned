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
                    iDParent: "6f85f39773",
                    priority: default,
                    comment: "61c050ef23e04c838cf9076688f6a"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"));
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
                    iDParent: "f2e5c07619474254b9e08",
                    priority: default,
                    comment: "2840f601bf3a457287571823223621608de445764bd240f6a86b05edbcd9effc25363b435116425a8f800"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}