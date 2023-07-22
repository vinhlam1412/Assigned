using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.Common.HQAssigneds
{
    public class HQAssignedsAppServiceTests : CommonApplicationTestBase
    {
        private readonly IHQAssignedsAppService _hQAssignedsAppService;
        private readonly IRepository<HQAssigned, Guid> _hQAssignedRepository;

        public HQAssignedsAppServiceTests()
        {
            _hQAssignedsAppService = GetRequiredService<IHQAssignedsAppService>();
            _hQAssignedRepository = GetRequiredService<IRepository<HQAssigned, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _hQAssignedsAppService.GetListAsync(new GetHQAssignedsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.HQAssigned.Id == Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae")).ShouldBe(true);
            result.Items.Any(x => x.HQAssigned.Id == Guid.Parse("dad45d4d-5471-4b58-b233-6044f271a07c")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hQAssignedsAppService.GetAsync(Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HQAssignedCreateDto
            {
                IDParent = "133893ae04774673b9141cc678f2cb6d973f1ef96051446c961897dcc560e0f1ca",
                Completeby = new DateTime(2008, 5, 4),
                Priority = default,
                Comment = "bc76a1779632441cbdf91a815e71ee668eb0f37e03af48eca2443ae0cdb3faaf7bacd0c23da14853baf6"
            };

            // Act
            var serviceResult = await _hQAssignedsAppService.CreateAsync(input);

            // Assert
            var result = await _hQAssignedRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe("133893ae04774673b9141cc678f2cb6d973f1ef96051446c961897dcc560e0f1ca");
            result.Completeby.ShouldBe(new DateTime(2008, 5, 4));
            result.Priority.ShouldBe(default);
            result.Comment.ShouldBe("bc76a1779632441cbdf91a815e71ee668eb0f37e03af48eca2443ae0cdb3faaf7bacd0c23da14853baf6");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HQAssignedUpdateDto()
            {
                IDParent = "b8f1db96f2d44f4ea771ffe676482666410ffd95a4da4ccc83f09527bb32b40e",
                Completeby = new DateTime(2010, 8, 9),
                Priority = default,
                Comment = "fa1d947e5ad64b899da6d7e0ea32b6d40f3a02136f84476daedef"
            };

            // Act
            var serviceResult = await _hQAssignedsAppService.UpdateAsync(Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"), input);

            // Assert
            var result = await _hQAssignedRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe("b8f1db96f2d44f4ea771ffe676482666410ffd95a4da4ccc83f09527bb32b40e");
            result.Completeby.ShouldBe(new DateTime(2010, 8, 9));
            result.Priority.ShouldBe(default);
            result.Comment.ShouldBe("fa1d947e5ad64b899da6d7e0ea32b6d40f3a02136f84476daedef");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hQAssignedsAppService.DeleteAsync(Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"));

            // Assert
            var result = await _hQAssignedRepository.FindAsync(c => c.Id == Guid.Parse("6637d576-1ce7-4d5d-a3bf-ad61a6b3a4ae"));

            result.ShouldBeNull();
        }
    }
}