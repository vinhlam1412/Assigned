using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.Common.HQShares;
using HQSOFT.Common.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.Common.HQShares
{
    public class HQShareRepositoryTests : CommonEntityFrameworkCoreTestBase
    {
        private readonly IHQShareRepository _hQShareRepository;

        public HQShareRepositoryTests()
        {
            _hQShareRepository = GetRequiredService<IHQShareRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQShareRepository.GetListAsync(
                    iDParent: "a54d307ca4544bc68fc6e9b5d70b651bcd34",
                    canRead: true,
                    canWrite: true,
                    canSubmit: true,
                    canShare: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hQShareRepository.GetCountAsync(
                    iDParent: "2552b6bb0625446c8c3e129d20aba84d443c5984c6974e85aabefd3bf4eb767eda5ae011380b4e76a57c07c91e5e",
                    canRead: true,
                    canWrite: true,
                    canSubmit: true,
                    canShare: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}