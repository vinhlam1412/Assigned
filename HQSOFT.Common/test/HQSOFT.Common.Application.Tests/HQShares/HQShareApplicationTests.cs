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
            result.Items.Any(x => x.HQShare.Id == Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4")).ShouldBe(true);
            result.Items.Any(x => x.HQShare.Id == Guid.Parse("8a14eeb2-4efd-426f-80f1-ab64826e6de2")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hQSharesAppService.GetAsync(Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HQShareCreateDto
            {
                IDParent = "67942d5c715440a8b8e3",
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
            result.IDParent.ShouldBe("67942d5c715440a8b8e3");
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
                IDParent = "ead1906af85143c6a",
                CanRead = true,
                CanWrite = true,
                CanSubmit = true,
                CanShare = true
            };

            // Act
            var serviceResult = await _hQSharesAppService.UpdateAsync(Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"), input);

            // Assert
            var result = await _hQShareRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe("ead1906af85143c6a");
            result.CanRead.ShouldBe(true);
            result.CanWrite.ShouldBe(true);
            result.CanSubmit.ShouldBe(true);
            result.CanShare.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hQSharesAppService.DeleteAsync(Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"));

            // Assert
            var result = await _hQShareRepository.FindAsync(c => c.Id == Guid.Parse("c69ece73-14fc-431f-b429-5dbe5cedcef4"));

            result.ShouldBeNull();
        }
    }
}