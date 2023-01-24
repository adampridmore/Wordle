using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

[Collection("Sequential")]
public class NewTeamTests : IClassFixture<TestWebApplicationFactory<Program>>
{
  private readonly TestWebApplicationFactory<Program> _factory;
  private readonly HttpClient _httpClient;

  public NewTeamTests(TestWebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _httpClient = factory.CreateClient();
  }

  [Fact]
  public async Task RegisterNewTeam()
  {
    using (var scope = _factory.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetService<WordleDb>();
      if (db != null)
      {
        await db.Database.EnsureCreatedAsync();
        if (db.Teams.Any())
        {
          db.Teams.RemoveRange(db.Teams);
          await db.SaveChangesAsync();
        }
      }
    }

    var client = _factory.WithWebHostBuilder(builder =>
    {
    }).CreateClient();

    var response = await client.PostAsJsonAsync("/team", new NewTeam
    {
      Name = "Test title"
    });

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }
}