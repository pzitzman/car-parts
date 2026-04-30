using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;

namespace CarParts.Tests;

public class CarPartIntegrattionTest : IClassFixture<CustomApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IMongoDatabase _testDB;

    public CarPartIntegrattionTest(CustomApiFactory factory)
    {
        _client = factory.CreateClient();
        var mongoClient = new MongoClient(
            "mongodb://admin:password@localhost:27017?authSource=admin"
        );
        _testDB = mongoClient.GetDatabase("CarParts-Test-DB");
    }

    public async Task InitializeAsync()
    {
        await _testDB.DropCollectionAsync("carparts");
    }

    public Task DisposeAsync() => Task.CompletedTask;

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
        var createdTempPart = await postResponse.Content.ReadFromJsonAsync<CarPartGetDto>();
        string testId = createdTempPart!.Id;

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

    // Test for bad name
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateWithBadName(string? badName)
    {
        var badDto = new
        {
            Name = badName,
            PartNumber = "9876F-ABC",
            Description = "Ein normaler Luftfilter",
        };

        var response = await _client.PostAsJsonAsync("/api/carpart", badDto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Part name is missing", errorString, StringComparison.OrdinalIgnoreCase);
    }

    // Test for bad part number
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateWithBadPartNumber(string? badPartNumber)
    {
        var badDto = new
        {
            Name = "Luftfilter",
            PartNumber = badPartNumber,
            Description = "Ein normaler Luftfilter",
        };

        var response = await _client.PostAsJsonAsync("/api/carpart", badDto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Part number is missing", errorString, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PositiveLoopNoDescription(string badDescription)
    {
        //POST
        var newPart = new
        {
            Name = "Ölfilter",
            PartNumber = "123Oel-ABC",
            Description = badDescription,
        };

        var response = await _client.PostAsJsonAsync("/api/carpart", newPart);
        Assert.True(response.IsSuccessStatusCode);

        var result = await response.Content.ReadFromJsonAsync<CarPartGetDto>();
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Description);

        string testId = result.Id;
        //DELETE
        var deleteResponse = await _client.DeleteAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteBeforUpdate()
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
        var createdTempPart = await postResponse.Content.ReadFromJsonAsync<CarPartGetDto>();
        string testId = createdTempPart!.Id;

        //GET
        var getResponse = await _client.GetAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        //DELETE
        var deleteResponse = await _client.DeleteAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        //Verfiy deletion
        var checkDeleted = await _client.GetAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.NotFound, checkDeleted.StatusCode);

        //PUT
        var partToUpdate = new
        {
            Name = "Luftfilter",
            PartNumber = "9876F-ABC",
            Description = "Ein normaler Luftfilter",
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/carpart/{testId}", partToUpdate);
        Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);

        var errorString = await putResponse.Content.ReadAsStringAsync();
        Assert.Contains($"Car Part not found with{testId}", errorString);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateGoodIdBadDto(string? badName)
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
        var createdTempPart = await postResponse.Content.ReadFromJsonAsync<CarPartGetDto>();
        string testId = createdTempPart!.Id;

        //GET
        var getResponse = await _client.GetAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        //PUT
        var partToUpdate = new
        {
            Name = badName,
            PartNumber = "9876F-ABC",
            Description = "Ein normaler Luftfilter",
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/carpart/{testId}", partToUpdate);
        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);

        var errorString = await putResponse.Content.ReadAsStringAsync();
        Assert.Contains("name", errorString, StringComparison.OrdinalIgnoreCase);

        //DELETE
        var deleteResponse = await _client.DeleteAsync($"/api/carpart/{testId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
