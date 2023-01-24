using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WordleAPI;

namespace WordleAPI.Tests.Helpers;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WordleDb>));

      if (descriptor != null)
      {
        services.Remove(descriptor);
      }

      services.AddDbContext<WordleDb>(options =>
          {
          var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
          var full = Path.Join(path, $"Wordle_tests.db");
          options.UseSqlite($"Data Source={full}");
        });
    });

    return base.CreateHost(builder);
  }
}