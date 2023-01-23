
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WordleDb>(opt => opt.UseInMemoryDatabase("WordleServer"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "WordleAPI!");

// Get Game

app.MapPost("/guess", async (NewGuess guessDetails, WordleDb db) =>
{
  var game = await db.Games.Where(t => t.Id == guessDetails.GameId)
                           .FirstOrDefaultAsync();
  if (game is null)
  {
    return Results.NotFound("Game does not exist. Please call Game first");
  }

  if (guessDetails.Guess.Length != 5) {
    return Results.BadRequest("Guesses must be 5 letters long");
  }

  if (game.Guess1 is null) {
    game.Guess1 = guessDetails.Guess;
  }
  else if (game.Guess2 is null) {
    game.Guess2 = guessDetails.Guess;
  }
  else if (game.Guess3 is null) {
    game.Guess3 = guessDetails.Guess;
  }
  else if (game.Guess4 is null) {
    game.Guess4 = guessDetails.Guess;
  }
  else if (game.Guess5 is null) {
    game.Guess5 = guessDetails.Guess;
  }
  else if (game.Guess6 is null) {
    game.Guess6 = guessDetails.Guess;
  }
  else 
  {
    return Results.BadRequest("You have already had 6 guesses at getting this word.");
  }

  await db.SaveChangesAsync();

  // TODO: Return matching letters

  return Results.Ok();
});


app.MapPost("/game", async (NewGame gameDetails, WordleDb db) =>
{
  var team = await db.Teams.Where(t => t.Id == gameDetails.TeamId)
                           .FirstOrDefaultAsync();
  if (team is null)
  {
    return Results.NotFound("Team does not exist. Please call Team first");
  }

  var game = new Game()
  {
    Id = Guid.NewGuid().ToString(),
    TeamId = team.Id,
    Word = ""  // TODO:  Pick a random word
  };

  db.Games.Add(game);
  await db.SaveChangesAsync();

  var result = new NewGameResponse() {
    GameId = game.Id
  };

  return Results.Ok(result);
});

app.MapPost("/team", async (NewTeam teamDetails, WordleDb db) =>
{
  var team = await db.Teams.Where(t => t.Name == teamDetails.Name)
                           .FirstOrDefaultAsync();
  if (team is null)
  {
    team = new Team()
    {
      Id = Guid.NewGuid().ToString(),
      Name = teamDetails.Name
    };

    db.Teams.Add(team);
    await db.SaveChangesAsync();
  }

  var result = new NewTeamResponse() {
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


app.Run();
