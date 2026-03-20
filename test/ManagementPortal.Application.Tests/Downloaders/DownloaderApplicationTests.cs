using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace ManagementPortal.Downloaders;

public abstract class DownloadersAppServiceTests<TStartupModule> : ManagementPortalApplicationTestBase<TStartupModule> where TStartupModule : IAbpModule
{
    private readonly IDownloadersAppService _downloadersAppService;
    private readonly IRepository<Downloader, Guid> _downloaderRepository;

    public DownloadersAppServiceTests()
    {
        _downloadersAppService = GetRequiredService<IDownloadersAppService>();
        _downloaderRepository = GetRequiredService<IRepository<Downloader, Guid>>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        // Act
        var result = await _downloadersAppService.GetListAsync(new GetDownloadersInput());
        // Assert
        result.TotalCount.ShouldBe(2);
        result.Items.Count.ShouldBe(2);
        result.Items.Any(x => x.Id == Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045")).ShouldBe(true);
        result.Items.Any(x => x.Id == Guid.Parse("73995b0c-980f-4218-94a3-d143c8fb6649")).ShouldBe(true);
    }

    [Fact]
    public async Task GetAsync()
    {
        // Act
        var result = await _downloadersAppService.GetAsync(Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"));
        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"));
    }

    [Fact]
    public async Task CreateAsync()
    {
        // Arrange
        var input = new DownloaderCreateDto
        {
            DownloaderEnabled = true,
            DownloaderPollarName = "6df6b4dda43a4480ad7443f9b1c4bf23e2be63eb86d947959d8ddd077bea5f2683b00aa2"
        };
        // Act
        var serviceResult = await _downloadersAppService.CreateAsync(input);
        // Assert
        var result = await _downloaderRepository.FindAsync(c => c.Id == serviceResult.Id);
        result.ShouldNotBe(null);
        result.DownloaderEnabled.ShouldBe(true);
        result.DownloaderPollarName.ShouldBe("6df6b4dda43a4480ad7443f9b1c4bf23e2be63eb86d947959d8ddd077bea5f2683b00aa2");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        // Arrange
        var input = new DownloaderUpdateDto()
        {
            DownloaderEnabled = true,
            DownloaderPollarName = "44bec02764644a8aa784e8e32a85f75a802746e652e342a8987d6b45cf06d0eb5021f60fd16442aface2173a8a17c"
        };
        // Act
        var serviceResult = await _downloadersAppService.UpdateAsync(Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"), input);
        // Assert
        var result = await _downloaderRepository.FindAsync(c => c.Id == serviceResult.Id);
        result.ShouldNotBe(null);
        result.DownloaderEnabled.ShouldBe(true);
        result.DownloaderPollarName.ShouldBe("44bec02764644a8aa784e8e32a85f75a802746e652e342a8987d6b45cf06d0eb5021f60fd16442aface2173a8a17c");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // Act
        await _downloadersAppService.DeleteAsync(Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"));
        // Assert
        var result = await _downloaderRepository.FindAsync(c => c.Id == Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"));
        result.ShouldBeNull();
    }
}
