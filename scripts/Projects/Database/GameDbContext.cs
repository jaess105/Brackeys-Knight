using Microsoft.EntityFrameworkCore;

namespace Database;

internal class GameDbContext(string dbPath) : DbContext
{
    public DbSet<RunData> RunData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={dbPath}");
    }
}
