using Microsoft.AspNetCore.Mvc.Testing;

namespace CarParts.Tests;

public class CarPartIntegradtionTest(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ProofOfLive_ApiResponds()
    {
        var response = await _client.GetAsync("/api/carpart");
        Assert.True(
            response.IsSuccessStatusCode
                || response.StatusCode == System.Net.HttpStatusCode.NotFound
        );
    }
}
