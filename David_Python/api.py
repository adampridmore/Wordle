import datetime
from dataclasses import dataclass
from enum import Enum
from typing import Final, List, NewType
from uuid import UUID

import requests

TeamId = NewType("TeamId", UUID)
GameId = NewType("GameId", UUID)

BASE_URL: Final = "https://yorkcodedojowordleapi.azurewebsites.net"


class GameState(Enum):
    InProgress = 0
    Won = 1
    Lost = 2


@dataclass(frozen=True)
class NewGuessResponse:
    Score: str
    State: GameState


@dataclass(frozen=True)
class GetGameScoreResponse:
    Guess: str
    Score: str


@dataclass(frozen=True)
class GetGameResponse:
    GameId: GameId
    Word: str
    DateStarted: datetime.datetime
    State: GameState
    Guesses: List[GetGameScoreResponse]


@dataclass()
class GetGamesResponse:
    GameId: GameId
    DateStarted: datetime.datetime
    State: GameState


def register_team(team_name: str) -> TeamId:
    payload = {
      "Name": team_name
    }
    response = requests.post(f"{BASE_URL}/team", json=payload)
    response.raise_for_status()
    return response.json()["id"]


def start_new_game(team_id: TeamId) -> GameId:
    payload = {
      "TeamId": team_id
    }
    response = requests.post(f"{BASE_URL}/game", json=payload)
    response.raise_for_status()
    return response.json()["gameId"]


def guess_word(game_id: GameId, guess: str) -> NewGuessResponse:
    payload = {
      "GameId": game_id,
      "Guess": guess
    }
    response = requests.post(f"{BASE_URL}/guess", json=payload)
    if response.status_code == 400:
        raise Exception(response.json())

    response.raise_for_status()
    return response.json()


def get_game(game_id: GameId) -> GetGameResponse:
    response = requests.get(f"{BASE_URL}/game/{game_id}")
    response.raise_for_status()
    return response.json()


def get_all_games(team_id: TeamId) -> GetGamesResponse:
    response = requests.get(f"{BASE_URL}/team/{team_id}/games")
    response.raise_for_status()
    return response.json()
