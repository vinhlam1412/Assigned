using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.Common.HQShares
{
    public class HQSharesAppServiceTests : CommonApplicationTestBase
    {
        private readonly IHQSharesAppService _hQSharesAppService;
        private readonly IRepository<HQShare, Guid> _hQShareRepository;

        public HQSharesAppServiceTests()
        {
            _hQSharesAppService = GetRequiredService<IHQSharesAppService>();
            _hQShareRepository = GetRequiredService<IRepository<HQShare, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _hQSharesAppService.GetListAsync(new GetHQSharesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.HQShare.Id == Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d")).ShouldBe(true);
            result.Items.Any(x => x.HQShare.Id == Guid.Parse("1813abc1-7745-4754-83cd-154677e5c689")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hQSharesAppService.GetAsync(Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HQShareCreateDto
            {
                IDParent = "03f09aa889784752ab379ad0f016b41f9b399864aa0b4de5bf7c83dbdcaa1edefc",
                CanRead = true,
                CanWrite = true,
                CanSubmit = true,
                CanShare = true
            };

            // Act
            var serviceResult = await _hQSharesAppService.CreateAsync(input);

            // Assert
            var result = await _hQShareRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe("03f09aa889784752ab379ad0f016b41f9b399864aa0b4de5bf7c83dbdcaa1edefc");
            result.CanRead.ShouldBe(true);
            result.CanWrite.ShouldBe(true);
            result.CanSubmit.ShouldBe(true);
            result.CanShare.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HQShareUpdateDto()
            {
                IDParent = "70f42e31fe5c4c5782cffb1c435c12aae58c",
                CanRead = true,
                CanWrite = true,
                CanSubmit = true,
                CanShare = true
            };

            // Act
            var serviceResult = await _hQSharesAppService.UpdateAsync(Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"), input);

            // Assert
            var result = await _hQShareRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe("70f42e31fe5c4c5782cffb1c435c12aae58c");
            result.CanRead.ShouldBe(true);
            result.CanWrite.ShouldBe(true);
            result.CanSubmit.ShouldBe(true);
            result.CanShare.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hQSharesAppService.DeleteAsync(Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"));

            // Assert
            var result = await _hQShareRepository.FindAsync(c => c.Id == Guid.Parse("716dafda-eddc-4e4b-8418-1a0a6b40466d"));

            result.ShouldBeNull();
        }
    }
}