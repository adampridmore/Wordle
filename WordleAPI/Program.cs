using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WordleDb>(opt => opt.UseInMemoryDatabase("WordleServer"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton(typeof(Words));
builder.Services.AddSingleton(typeof(Scorer));
builder.Services.AddSingleton(typeof(ValidationTools));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapTeamEndpoints();
app.MapGameEndpoints();
app.MapGuessEndpoints();

app.MapGet("/", () => "WordleAPI!");

app.Run();



public partial class Program
{ }