using Microsoft.EntityFrameworkCore;

public class WordleDb : DbContext
{
    public WordleDb(DbContextOptions<WordleDb> options)
        : base(options) { }

    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Game> Games => Set<Game>();
}