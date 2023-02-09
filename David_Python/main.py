import api

team_id = api.register_team("Python Test")
print(team_id)

game_id = api.start_new_game(team_id)
print(game_id)

guess_response = api.guess_word(game_id, "APPLE")
print(guess_response)

current_game = api.get_game(game_id)
print(current_game)

all_games = api.get_all_games(team_id)
print(all_games)
