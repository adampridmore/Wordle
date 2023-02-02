using System.Net.Http.Json;

public class API
{
  const string baseURL = "https://yorkcodedojowordleapi.azurewebsites.net";
  static readonly HttpClient client = new HttpClient();

  public async Task<Guid> RegisterTeam(string teamName)
  {
    var payload = new NewTeam
    {
      Name = teamName
    };

    var response = await Post<NewTeamResponse>("team", payload);
    return response.Id;
  }

  public async Task<Guid> StartNewGame(Guid teamId)
  {
    var payload = new NewGame
    {
      TeamId = teamId
    };

    var response = await Post<NewGameResponse>("game", payload);
    return response.GameId;
  }

  public async Task<NewGuessResponse> GuessWord(Guid gameId, string guess)
  {
    var payload = new NewGuess
    {
      GameId = gameId,
      Guess = guess
    };

    var response = await Post<NewGuessResponse>("guess", payload);
    return response;
  }


  public async Task<GetGameResponse> GetGame(Guid gameId)
  {
    var url = $"{baseURL}/game/{gameId}";
    var result = await client.GetFromJsonAsync<GetGameResponse>(url);
    if (result is null)
      throw new Exception($"No result returned by GET call to game");
    return result;
  }

  private async Task<TResponse> Post<TResponse>(string endpoint, object payload)
  {
    var url = $"{baseURL}/{endpoint}";
    var httpResponse = await client.PostAsJsonAsync(url, payload);

    if (!httpResponse.IsSuccessStatusCode)
    {
      var details = await httpResponse.Content.ReadAsStringAsync();
      throw new Exception(details);
    }

    var result = await httpResponse.Content.ReadFromJsonAsync<TResponse>();
    if (result is null)
      throw new Exception($"No result returned by POST call to {endpoint}");
    return result;
  }
}