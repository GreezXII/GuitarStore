using GuitarStore.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GuitarStore.Tests;

[TestClass]
public class RestApiTests
{
    private GuitarStoreClient? _guitarsStoreClient;
    private readonly WebApplicationFactory<Program> _factory;

    public RestApiTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }
    
    [TestMethod]
    public async Task Get_AllGuitars_Success()
    {
        // Arrange
        var client = _factory.CreateClient();
        _guitarsStoreClient = new GuitarStoreClient(client.BaseAddress!.ToString(), client);
        
        // Act
        // var response = await client.GetAsync("Guitars/GetAllGuitars");
        var result = await _guitarsStoreClient.GetAllGuitarsAsync();        
        // Assert
        Assert.IsNotNull(result);
    }
}