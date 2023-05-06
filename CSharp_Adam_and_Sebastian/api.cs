namespace Wordle;

using System.Net.Http.Json;
using Wordle.Models;

public class Api
{
  const string BaseUrl = "https://yorkcodedojowordleapi.azurewebsites.net";
  static readonly HttpClient Client = new HttpClient();

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


  public async Task DownloadWords(string path)
  {
    const string url = $"{BaseUrl}/words.txt";
    var response = await Client.GetAsync(url);
    response.EnsureSuccessStatusCode();
    var stream = await response.Content.ReadAsStreamAsync();
    var fileInfo = new FileInfo(path);
    var fileStream = fileInfo.OpenWrite();
    await using (fileStream.ConfigureAwait(false))
    {
      await stream.CopyToAsync(fileStream);
    }
  }

  public async Task<GetGameResponse> GetGame(Guid gameId)
  {
    var url = $"{BaseUrl}/game/{gameId}";
    var result = await Client.GetFromJsonAsync<GetGameResponse>(url);
    if (result is null)
      throw new Exception($"No result returned by GET call to game");
    return result;
  }

  private async Task<TResponse> Post<TResponse>(string endpoint, object payload)
  {
    var url = $"{BaseUrl}/{endpoint}";
    var httpResponse = await Client.PostAsJsonAsync(url, payload);

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
