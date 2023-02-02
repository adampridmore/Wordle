using System.Net.Http.Json;

public class API
{
  static readonly HttpClient client = new HttpClient();

  public async Task<Guid> RegisterTeam(string teamName)
  {
    var payload = new NewTeam
    {
      Name = teamName
    };

    var response = await POST<NewTeamResponse>("team", payload);
    return response!.Id;
  }

  public async Task<Guid> StartNewGame(Guid teamId)
  {
    var payload = new NewGame
    {
       TeamId = teamId
    };

    var response = await POST<NewGameResponse>("game", payload);
    return response!.GameId;
  }

  private async Task<TResponse?> POST<TResponse>(string endpoint, object payload)
  {
    var url = $"https://yorkcodedojowordleapi.azurewebsites.net/{endpoint}";
    var httpResponse = await client.PostAsJsonAsync(url, payload);
    httpResponse.EnsureSuccessStatusCode();

    return await httpResponse.Content.ReadFromJsonAsync<TResponse>();
  }
}