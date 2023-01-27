dotnet publish -c Release -o ./bin/Publish
Right click on publish and deploy to web app

dotnet tool install --global dotnet-ef 
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add DateStarted             
dotnet ef migrations script --output DateStarted.sql
dotnet ef migrations script InitialCreate  --output DateStarted.sql 

https://yorkcodedojowordleapi.azurewebsites.net/words.txt

## TODO

Add date created to Game
Add API to list games in date order

Example solution

Admin methods for me
  List of teams
  Dashboard?