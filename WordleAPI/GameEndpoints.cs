using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public static class GameEndpoints
{
  public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapPost("/game", CreateNewGame)
             .WithName("NewGame")
             .WithOpenApi(operation => new(operation)
             {
               Summary = "Starts a new game.",
               Description = "Please register your team to obtain a TeamId first"
             });

    endpoints.MapGet("/game/{gameId}", GetGame)
             .WithName("GetGame")
             .WithOpenApi(operation => new(operation)
             {
               Summary = "Retrieves the details of your game.",
               Description = "Includes previous guesses and their scores."
             });

    endpoints.MapGet("/team/{teamId}/games", GetGames)
             .WithName("GetGames")
             .WithOpenApi(operation => new(operation)
             {
               Summary = "Retrieves the details of all your games.",
               Description = ""
             });

    return endpoints;
  }


  async static Task<Results<Ok<GetGamesResponse[]>, NotFound<string>>> GetGames(WordleDb db, Guid teamId)
  {
    var team = await db.Teams.Where(t => t.Id == teamId)
                            .FirstOrDefaultAsync();

    if (team is null || teamId == Guid.Empty)
    {
      return TypedResults.NotFound("The team does not exist.");
    }

    var games = await db.Games.Where(t => t.TeamId == teamId)
                              .Select(t => new GetGamesResponse{
                                GameId = t.Id,
                                DateStarted = t.DateStarted,
                                State = t.State
                              })
                              .OrderBy(t => t.DateStarted)
                              .ToArrayAsync();

    return TypedResults.Ok(games);
  }



  async static Task<Results<Ok<GetGameResponse>, NotFound<string>>> GetGame(WordleDb db, Guid gameId, Scorer scorer)
  {
    var game = await db.Games.Where(t => t.Id == gameId)
                                .FirstOrDefaultAsync();

    if (game is null || gameId == Guid.Empty)
    {
      return TypedResults.NotFound("The game does not exist.");
    }

    var result = new GetGameResponse
    {
      GameId = game.Id,
      Word = string.Empty,
      State = game.State,
      DateStarted = game.DateStarted
    };

    if (game.State != GameState.InProgress)
    {
      result.Word = game.Word;
    }

    var guesses = new List<GetGameResponse.GetGameScoreResponse>();
    AddGuess(scorer, game.Guess1, game.Word, guesses);
    AddGuess(scorer, game.Guess2, game.Word, guesses);
    AddGuess(scorer, game.Guess3, game.Word, guesses);
    AddGuess(scorer, game.Guess4, game.Word, guesses);
    AddGuess(scorer, game.Guess5, game.Word, guesses);
    AddGuess(scorer, game.Guess6, game.Word, guesses);
    result.Guesses = guesses.ToArray();

    return TypedResults.Ok(result);

    static void AddGuess(Scorer scorer, string? guess, string actualWord, List<GetGameResponse.GetGameScoreResponse> guesses)
    {
      if (guess is not null)
      {
        guesses.Add(new GetGameResponse.GetGameScoreResponse
        {
          Guess = guess,
          Score = scorer.ScoreGuess(actualWord, guess)
        });
      }
    }
  }

  async static Task<Results<Ok<NewGameResponse>, NotFound<string>>> CreateNewGame(NewGame gameDetails,
                                                                                  WordleDb db,
                                                                                  Words words)
  {
    var team = await db.Teams.Where(t => t.Id == gameDetails.TeamId)
                             .FirstOrDefaultAsync();
    if (team is null || gameDetails.TeamId == Guid.Empty)
    {
      return TypedResults.NotFound("Team does not exist. Please call Team first.");
    }

    var game = new Game()
    {
      Id = Guid.NewGuid(),
      Team = team,
      State = GameState.InProgress,
      Word = words.RandomWord(),
      DateStarted = DateTime.Now
    };

    db.Games.Add(game);
    await db.SaveChangesAsync();

    var result = new NewGameResponse()
    {
      GameId = game.Id
    };

    return TypedResults.Ok(result);
  }
}