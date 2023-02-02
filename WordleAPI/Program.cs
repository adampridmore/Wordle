using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<WordleDb>(opt => opt.UseInMemoryDatabase("WordleServer"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton(new Words(builder.Environment.WebRootPath));
builder.Services.AddSingleton<Scorer>();
builder.Services.AddSingleton<ValidationTools>();

builder.Services.AddDbContext<WordleDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Azure"));
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

app.MapTeamEndpoints();
app.MapGameEndpoints();
app.MapGuessEndpoints();

app.MapGet("/", () => Results.Redirect("/Swagger")).ExcludeFromDescription();

app.Run();



public partial class Program
{ }