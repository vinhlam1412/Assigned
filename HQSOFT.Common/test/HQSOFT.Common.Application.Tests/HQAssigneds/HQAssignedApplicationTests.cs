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
            result.Items.Any(x => x.HQAssigned.Id == Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0")).ShouldBe(true);
            result.Items.Any(x => x.HQAssigned.Id == Guid.Parse("013e7d8f-568f-4b0f-8516-b20d6b952e8a")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hQAssignedsAppService.GetAsync(Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HQAssignedCreateDto
            {
                IDParent = Guid.Parse("632ef2bf-a893-4c2a-b5d7-f7f2fd14fa1e"),
                Completeby = new DateTime(2002, 6, 20),
                Priority = default,
                Comment = "3bf48ca2a1624400b698"
            };

            // Act
            var serviceResult = await _hQAssignedsAppService.CreateAsync(input);

            // Assert
            var result = await _hQAssignedRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe(Guid.Parse("632ef2bf-a893-4c2a-b5d7-f7f2fd14fa1e"));
            result.Completeby.ShouldBe(new DateTime(2002, 6, 20));
            result.Priority.ShouldBe(default);
            result.Comment.ShouldBe("3bf48ca2a1624400b698");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HQAssignedUpdateDto()
            {
                IDParent = Guid.Parse("0e94ef94-18d9-4cb0-837a-5d4986f2e451"),
                Completeby = new DateTime(2022, 9, 20),
                Priority = default,
                Comment = "4ee1486ca2534618b463dd4b2fb9f2c5a6ff3ebe782f4a98aa48b148"
            };

            // Act
            var serviceResult = await _hQAssignedsAppService.UpdateAsync(Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"), input);

            // Assert
            var result = await _hQAssignedRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.IDParent.ShouldBe(Guid.Parse("0e94ef94-18d9-4cb0-837a-5d4986f2e451"));
            result.Completeby.ShouldBe(new DateTime(2022, 9, 20));
            result.Priority.ShouldBe(default);
            result.Comment.ShouldBe("4ee1486ca2534618b463dd4b2fb9f2c5a6ff3ebe782f4a98aa48b148");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hQAssignedsAppService.DeleteAsync(Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"));

            // Assert
            var result = await _hQAssignedRepository.FindAsync(c => c.Id == Guid.Parse("e3bdf913-c647-4bab-998c-48f3974187f0"));

            result.ShouldBeNull();
        }
    }
}