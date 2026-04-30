using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualBasic;

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

    [Fact]
    public async Task PositiveLoop()
    {
        //POST
        var newPart = new
        {
            Name = "Ölfilter",
            PartNumber = "123Oel-ABC",
            Description = "Ein normaler Ölfilter",
        };

        var postResponse = await _client.PostAsJsonAsync("/api/carpart", newPart);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        //Get id from response
        var createdTempPart = await postResponse.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(createdTempPart);
        string testId =
            createdTempPart.RootElement.GetProperty("id").GetString()
            ?? throw new InvalidOperationException("The Api did not return an Id from Json");

        //GET
        var getResponse = await _client.GetAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        //PUT
        var partToUpdate = new
        {
            Name = "Luftfilter",
            PartNumber = "9876F-ABC",
            Description = "Ein normaler Luftfilter",
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/carpart/{testId}", partToUpdate);
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        //DELETE
        var deleteResponse = await _client.DeleteAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        //Verfiy deletion
        var checkDeleted = await _client.GetAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.NotFound, checkDeleted.StatusCode);
    }
}
