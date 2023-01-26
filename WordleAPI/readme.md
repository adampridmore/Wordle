dotnet publish -c Release -o ./bin/Publish
Right click on publish and deploy to web app

dotnet tool install --global dotnet-ef 
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate             
dotnet ef migrations script --output InitialCreate.sql

https://yorkcodedojowordleapi.azurewebsites.net/words.txt

## TODO

Get method should'nt only include the word unless the game is complete

Add date created to Game
Add API to list games

Example solution

Admin methods for me
  List of teams
  Dashboard?