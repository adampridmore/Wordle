using Microsoft.EntityFrameworkCore;

public static class GameEndpoints
{
  public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapPost("/game", async (NewGame gameDetails, WordleDb db, Words words) =>
    {
      var team = await db.Teams.Where(t => t.Id == gameDetails.TeamId)
                               .FirstOrDefaultAsync();
      if (team is null || gameDetails.TeamId == Guid.Empty)
      {
        return Results.NotFound("Team does not exist. Please call Team first.");
      }

      var game = new Game()
      {
        Id = Guid.NewGuid(),
        Team = team,
        State = GameState.InProgress,
        Word = words.RandomWord()
      };

      db.Games.Add(game);
      await db.SaveChangesAsync();

      var result = new NewGameResponse()
      {
        GameId = game.Id
      };

      return Results.Ok(result);
    });
    return endpoints;
  }
}