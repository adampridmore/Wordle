class GetGameScoreResponse
{
      public string? Guess { get; set; }
      public string? Score { get; set; }
}

class GetGameResponse
{
      public string GameId { get; set; } = "";
      public string Word { get; set; } = "";  // Only populated when state!=inprogress
      public string State { get; set; } = ""; //TODO: Use enum
      public GetGameScoreResponse[] Guesses { get; set; } = new GetGameScoreResponse[]{};
}