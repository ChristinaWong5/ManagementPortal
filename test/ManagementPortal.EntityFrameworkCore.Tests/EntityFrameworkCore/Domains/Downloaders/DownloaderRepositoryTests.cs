using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using ManagementPortal.Downloaders;
using ManagementPortal.EntityFrameworkCore;
using Xunit;

namespace ManagementPortal.EntityFrameworkCore.Domains.Downloaders;

public class DownloaderRepositoryTests : ManagementPortalEntityFrameworkCoreTestBase
{
    private readonly IDownloaderRepository _downloaderRepository;

    public DownloaderRepositoryTests()
    {
        _downloaderRepository = GetRequiredService<IDownloaderRepository>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        // Arrange
        await WithUnitOfWorkAsync(async () => {
            // Act
            var result = await _downloaderRepository.GetListAsync(downloaderEnabled: true, downloaderPollarName: "c7fe11b1c8a74856abfc18578488961678968c7827514329b55a1b0bd7e5f1a4fe1263274d8c406388c33622dd99409f83");
            // Assert
            result.Count.ShouldBe(1);
            result.FirstOrDefault().ShouldNotBe(null);
            result.First().Id.ShouldBe(Guid.Parse("3deb1dd7-f172-4a7d-9955-73eedffdc045"));
        });
    }

    [Fact]
    public async Task GetCountAsync()
    {
        // Arrange
        await WithUnitOfWorkAsync(async () => {
            // Act
            var result = await _downloaderRepository.GetCountAsync(downloaderEnabled: true, downloaderPollarName: "0326309dcebb44f59599e3c3412f0dd38680312");
            // Assert
            result.ShouldBe(1);
        });
    }
}
