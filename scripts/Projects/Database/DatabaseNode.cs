
namespace Database;

public interface IRunDataRepository
{
    IEnumerable<RunData> GetPreviousRunData();
}

public class MainRepository : IRunDataRepository
{
    private static string _dbPath = null!;

    public static void Init(string dbPath)
    {
        if (_dbPath is not null) { return; }
        _dbPath = dbPath;

        using GameDbContext db = new(_dbPath);
        db.Database.EnsureDeleted();   // delete old DB
        db.Database.EnsureCreated();   // create fresh DB matching current model
    }

    public static MainRepository Instance { get; } = new();

    private MainRepository() { }

    private static GameDbContext Db() => new(_dbPath);

    public IEnumerable<RunData> GetPreviousRunData()
    {
        using GameDbContext db = Db();
        if (!db.RunData.Any())
        {
            db.RunData.Add(new RunData { PlayerName = "Alice", CollectedCoins = 42, TimeToFinish = TimeSpan.FromSeconds(120) });
            db.SaveChanges();
        }

        return [.. db.RunData];
    }
}