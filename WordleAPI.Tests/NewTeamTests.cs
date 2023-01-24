using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

public class NewTeamTests : BaseTest
{
  public NewTeamTests(TestWebApplicationFactory<Program> factory) : base(factory) { }


  [Fact]
  public async Task RegisterNewTeam()
  {
    var client = await Given();

    // When a new team is registered
    var response = await client.PostAsJsonAsync("/team", new NewTeam
    {
      Name = "Test title"
    });

    Then(async context => {
      // Then the team is added to the database.
      var createdTeam = context!.Teams.First();
      Assert.Equal(createdTeam.Name, "Test title");

      // Then the details of the team are returned
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      var detail = await response.Content.ReadFromJsonAsync<NewTeamResponse>();
      Assert.NotNull(detail);
      Assert.Equal("Test title", detail.Name);
      Assert.Equal(createdTeam.Id, detail.Id);
    });

  }

  [Fact]
  public async Task ReRegisterExistingTeam()
  {
    var teamId = Guid.NewGuid();
    var client = await Given(context =>
    {
      context.Teams.Add(new Team
      {
        Id = teamId,
        Name = "Test title"
      });
    });

    var response = await client.PostAsJsonAsync("/team", new NewTeam
    {
      Name = "Test title"
    });

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<NewTeamResponse>();
    Assert.NotNull(detail);
    Assert.Equal("Test title", detail.Name);
    Assert.Equal(teamId, detail.Id);
  }
}