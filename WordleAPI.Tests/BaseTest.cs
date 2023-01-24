using Microsoft.Extensions.DependencyInjection;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

[Collection("all")]
public abstract class BaseTest : IClassFixture<TestWebApplicationFactory<Program>>
{
  private readonly TestWebApplicationFactory<Program> _factory;
  private readonly HttpClient _httpClient;

  public BaseTest(TestWebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _httpClient = factory.CreateClient();
  }

  protected async Task Then(Func<WordleDb, Task> check)
  {
    using (var scope = _factory.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetService<WordleDb>();
      await check(db!);
    }
  }

  protected async Task<HttpClient> Given(Action<WordleDb>? builder = null)
  {
    using (var scope = _factory.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetService<WordleDb>();
      if (db != null)
      {
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        if (builder is not null)
        {
          builder(db);
        }
        await db.SaveChangesAsync();
      }
    }

  var client = _factory.WithWebHostBuilder(builder =>
  {
  }).CreateClient();
    return client;
  }
}
