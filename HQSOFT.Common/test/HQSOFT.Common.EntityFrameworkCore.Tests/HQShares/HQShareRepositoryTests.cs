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
                    iDParent: "a7952fe98f8645f3856ce6da3e84bc03bd10df7ba2bd4f5c9ce4d0782",
                    canRead: true,
                    canWrite: true,
                    canSubmit: true,
                    canShare: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"));
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
                    iDParent: "1609b18",
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