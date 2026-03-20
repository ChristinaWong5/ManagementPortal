using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ManagementPortal.Pages;

[Collection(ManagementPortalTestConsts.CollectionDefinitionName)]
public class Index_Tests : ManagementPortalWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}

