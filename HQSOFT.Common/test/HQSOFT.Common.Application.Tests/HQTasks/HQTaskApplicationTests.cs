using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.Common.HQTasks
{
    public class HQTasksAppServiceTests : CommonApplicationTestBase
    {
        private readonly IHQTasksAppService _hQTasksAppService;
        private readonly IRepository<HQTask, Guid> _hQTaskRepository;

        public HQTasksAppServiceTests()
        {
            _hQTasksAppService = GetRequiredService<IHQTasksAppService>();
            _hQTaskRepository = GetRequiredService<IRepository<HQTask, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _hQTasksAppService.GetListAsync(new GetHQTasksInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("15b45a77-923c-4a36-9be3-4e900edfac50")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hQTasksAppService.GetAsync(Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HQTaskCreateDto
            {
                Subject = "2abe33c7d5a644f789da9bd856f90a6e96d0fac58b0743429",
                Project = "095031a3528a4680aad2083f2",
                Status = default,
                Priority = default,
                ExpectedStartDate = new DateTime(2000, 1, 17),
                ExpectedEndDate = new DateTime(2022, 1, 24),
                ExpectedTime = 440294334,
                Process = 1485580048
            };

            // Act
            var serviceResult = await _hQTasksAppService.CreateAsync(input);

            // Assert
            var result = await _hQTaskRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Subject.ShouldBe("2abe33c7d5a644f789da9bd856f90a6e96d0fac58b0743429");
            result.Project.ShouldBe("095031a3528a4680aad2083f2");
            result.Status.ShouldBe(default);
            result.Priority.ShouldBe(default);
            result.ExpectedStartDate.ShouldBe(new DateTime(2000, 1, 17));
            result.ExpectedEndDate.ShouldBe(new DateTime(2022, 1, 24));
            result.ExpectedTime.ShouldBe(440294334);
            result.Process.ShouldBe(1485580048);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HQTaskUpdateDto()
            {
                Subject = "59dba86264bf438fb1f63df61145efc69fdb2ad7b9ed486b9e936b8995c51b6a23d67",
                Project = "1f190bf51d01421094ade3f68e08f658eba553838d774ceda27b7e619da22c3ac6c6df9d",
                Status = default,
                Priority = default,
                ExpectedStartDate = new DateTime(2006, 2, 19),
                ExpectedEndDate = new DateTime(2011, 8, 25),
                ExpectedTime = 1315048798,
                Process = 1569587140
            };

            // Act
            var serviceResult = await _hQTasksAppService.UpdateAsync(Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"), input);

            // Assert
            var result = await _hQTaskRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Subject.ShouldBe("59dba86264bf438fb1f63df61145efc69fdb2ad7b9ed486b9e936b8995c51b6a23d67");
            result.Project.ShouldBe("1f190bf51d01421094ade3f68e08f658eba553838d774ceda27b7e619da22c3ac6c6df9d");
            result.Status.ShouldBe(default);
            result.Priority.ShouldBe(default);
            result.ExpectedStartDate.ShouldBe(new DateTime(2006, 2, 19));
            result.ExpectedEndDate.ShouldBe(new DateTime(2011, 8, 25));
            result.ExpectedTime.ShouldBe(1315048798);
            result.Process.ShouldBe(1569587140);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hQTasksAppService.DeleteAsync(Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"));

            // Assert
            var result = await _hQTaskRepository.FindAsync(c => c.Id == Guid.Parse("e668fc4c-8d53-4b4a-ada0-226a3155cf73"));

            result.ShouldBeNull();
        }
    }
}