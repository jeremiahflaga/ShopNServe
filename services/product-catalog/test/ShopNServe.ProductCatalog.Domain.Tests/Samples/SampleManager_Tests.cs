using System.Threading.Tasks;
using Xunit;

namespace ShopNServe.ProductCatalog.Samples;

public class SampleManager_Tests : ProductCatalogDomainTestBase
{
    //private readonly SampleManager _sampleManager;

    public SampleManager_Tests()
    {
        //_sampleManager = GetRequiredService<SampleManager>();
    }

    [Fact]
    public Task Method1Async()
    {
        return Task.CompletedTask;
    }
}
