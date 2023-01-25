using Microsoft.EntityFrameworkCore;

public static class TeamEndpoints
{
  public static IEndpointRouteBuilder MapTeamEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapPost("/team", async (NewTeam teamDetails, WordleDb db) =>
    {
      if (teamDetails.Name is null || teamDetails.Name.Length < 1 || teamDetails.Name.Length > 50)
      {
        return Results.BadRequest("The team name must be between 1 and 50 characters.");
      }

      var team = await db.Teams.Where(t => t.Name == teamDetails.Name)
                               .FirstOrDefaultAsync();
      if (team is null)
      {
        team = new Team()
        {
          Id = Guid.NewGuid(),
          Name = teamDetails.Name
        };

        db.Teams.Add(team);
        await db.SaveChangesAsync();
      }

      var result = new NewTeamResponse()
      {
        Id = team.Id,
        Name = teamDetails.Name
      };

      return Results.Ok(result);
    })
      .WithName("RegisterTeam")
      .WithOpenApi(operation => new(operation)
      {
        Summary = "Register your team with the server",
        Description = "The name of the team must be unique.  If the team already exists then the existing ID will be returned."
      });

    return endpoints;
  }
}