using Application.Common.Models;
using Application.Lists.Commands.CreateList;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests;

public class ListsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ListsControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateList_ValidRequest_ReturnsOk()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        var request = new CreateListCommand(new List<PointModel>().ToHashSet());

        // Act
        var response = await _client.PostAsync("/api/Lists", GetStringContent(request));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateList_ValidRequest_ReturnsNotNullResponse()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        var request = new CreateListCommand(new List<PointModel>().ToHashSet());

        // Act
        var response = await _client.PostAsync("/api/Lists", GetStringContent(request));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var createListResponse = JsonSerializer.Deserialize<CreateListResponse>(content, _jsonSerializerOptions);

        Assert.NotNull(createListResponse);
    }

    [Fact]
    public async Task AddPoint_NonExistingList_ReturnsNotFound()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        // Act
        var response = await _client.PutAsync(
            $"/api/Lists/add-point?ListId={123}&Point.X={0}&Point.Y={0}", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddPoint_ExistingList_ReturnsOk()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        var creatListRequest = new CreateListCommand(new List<PointModel>().ToHashSet());
        var createListResponse = await CreateListAsync(creatListRequest);

        // Act
        var response = await _client.PutAsync(
            $"/api/Lists/add-point?ListId={createListResponse.ListId}&Point.X={0}&Point.Y={0}", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeletePoint_NonExistingList_ReturnsNotFound()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/Lists/delete-point?ListId={123}&Point.X={0}&Point.Y={0}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePoint_ExistingList_ReturnsOk()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        var creatListRequest = new CreateListCommand(new List<PointModel>() { new PointModel(0, 0) }.ToHashSet());
        var createListResponse = await CreateListAsync(creatListRequest);

        // Act
        var response = await _client.DeleteAsync(
            $"/api/Lists/delete-point?ListId={createListResponse.ListId}&Point.X={0}&Point.Y={0}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetIdentifiedSquares_NonExistingList_ReturnsNotFound()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        // Act
        var response = await _client.GetAsync(
            $"/api/Lists/get-identified-squares?ListId={123}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetIdentifiedSquares_ExistingList_ReturnsOk()
    {
        // Arrange
        await NavigateToSwaggerAsync();

        var createListRequest = new CreateListCommand(new List<PointModel>().ToHashSet());
        var createListResponse = await CreateListAsync(createListRequest);

        // Act
        var response = await _client.GetAsync(
            $"/api/Lists/get-identified-squares?ListId={createListResponse.ListId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static StringContent GetStringContent<TType>(TType request)
    {
        return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
    }

    private async Task<CreateListResponse> CreateListAsync(CreateListCommand request)
    {
        var response = await _client.PostAsync("/api/Lists", GetStringContent(request));

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreateListResponse>(content, _jsonSerializerOptions)!;
    }

    private async Task NavigateToSwaggerAsync()
    {
        var response = await _client.GetAsync("/swagger/index.html");
        response.EnsureSuccessStatusCode();
    }
}
