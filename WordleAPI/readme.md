dotnet publish -c Release -o ./bin/Publish
Right click on publish and deploy to web app

dotnet tool install --global dotnet-ef 
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate             
dotnet ef migrations script --output InitialCreate.sql


## TODO

Add date created to Game
Add API to list games

Add tables to SQL Server
Connect app to sql server

Remove hardcoded file path to words
Example solution

Admin methods for me
  List of teams
  Dashboard?