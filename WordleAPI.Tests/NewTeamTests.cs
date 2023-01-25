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
    const string validTeamName = "Test Title";
    var response = await WhenTheTeamIsCreated(validTeamName);

    await ThenOKIsReturned(response);
    
    // Then the details of the team are returned
    var detail = await response.Content.ReadFromJsonAsync<NewTeamResponse>();
    Assert.NotNull(detail);

    // Then the team is added to the database.
    Then(context => {
      var createdTeam = context.Teams.First();
      Assert.Equal(createdTeam.Name, validTeamName);
      Assert.Equal(validTeamName, detail.Name);
      Assert.Equal(createdTeam.Id, detail.Id);
    });
  }

  [Fact]
  public async Task ReRegisterExistingTeam()
  {
    var teamId = Guid.NewGuid();
    const string validTeamName = "Test Title";
    await Given(context =>
    {
      var team = new Team
      {
        Id = teamId,
        Name = validTeamName
      };
      context.Teams.Add(team);
      return team;
    });

    var response = await WhenTheTeamIsCreated(validTeamName);

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewTeamResponse>();
    Assert.NotNull(detail);
    Assert.Equal(validTeamName, detail.Name);
    Assert.Equal(teamId, detail.Id);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("123456789012345678901234567890123456789012345678901")]
  public async Task TeamsMustBeGivenTeamNames(string teamNameBeingTested)
  {
    var response = await WhenTheTeamIsCreated(teamNameBeingTested);

    await ThenBadRequestIsReturned(response, "The team name must be between 1 and 50 characters.");
  }

  private async Task<HttpResponseMessage> WhenTheTeamIsCreated(string teamNameBeingTested)
  {
    var client = Client();
    var response = await client.PostAsJsonAsync("/team", new NewTeam
    {
      Name = teamNameBeingTested
    });
    return response;
  }
}