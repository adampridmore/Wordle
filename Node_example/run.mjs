
const createTeam = async () => {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: 'Node example' })
    };
    const response = await fetch('https://yorkcodedojowordleapi.azurewebsites.net/team', requestOptions);

    if (!response.ok) {
        throw Error("Error calling create team - " + response.status)
    }

    const data = await response.json();
    const teamId = data.id;
    return teamId;
}

const startGame = async (teamId) => {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ teamid: teamId })
    };
    const response = await fetch('https://yorkcodedojowordleapi.azurewebsites.net/game', requestOptions);
    if (!response.ok) {
        throw Error("Error calling start game - " + response.status)
    }

    const data = await response.json();
    const gameId = data.gameId;
    return gameId;
}

const play = async () => {
    const teamId = await createTeam();
    const gameId = await startGame(teamId);
    console.log(gameId);
}


try
{
    await play();
}
catch (e) {
    console.log(e);
}

