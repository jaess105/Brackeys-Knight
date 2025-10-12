using System.Data;
using Dapper;
using Database.Core;
using Database.Core.Migrations;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;

namespace Database;

public interface IRunDataRepository
{
    IEnumerable<RunData> GetPreviousRunData();
    RunData Insert(RunData runData);
}

public class MainRepository : IRunDataRepository
{
    private static IDbConnectionFactory _connectionFactory = null!;

    public static void Init(string dbPath)
    {
        if (_connectionFactory is not null) { return; }

        string connectionString = $"Data source={dbPath}";
        new MigrationRunner(connectionString).RunMigrations();
        _connectionFactory = new ConnectionFactory(connectionString);
    }

    public static MainRepository Instance { get; } = new();

    private MainRepository() { }

    public IEnumerable<RunData> GetPreviousRunData()
    {
        var col = GetAll().ToArray();
        if (col.IsNullOrEmpty())
        {
            var player = new RunData(Id: null, PlayerName: "Jannik", LevelName: "LevelInfinite", CollectedCoins: 1, TimeToFinish: TimeSpan.FromSeconds(240));
            Create(ref player);
        }
        return GetAll();
    }


    public void Create(ref RunData run)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            connection.Open();

            const string sql =
            """
            INSERT INTO RunData (PlayerName, LevelName, CollectedCoins, TimeToFinishMs)
            VALUES (@PlayerName, @LevelName, @CollectedCoins, @TimeToFinishMs);
            SELECT last_insert_rowid();
            """;

            // Convert TimeSpan → milliseconds for SQLite
            var dto = RunDataDto.FromDomain(run);
            var id = connection.ExecuteScalar<long>(sql, dto);

            run = run with { Id = id };
            connection.Close();
        }
    }

    public IEnumerable<RunData> GetAll()
    {
        IEnumerable<RunData> res;
        using (var connection = _connectionFactory.CreateConnection())
        {
            connection.Open();

            var sql = "SELECT Id, PlayerName, CollectedCoins, TimeToFinishMs FROM RunData;";
            var rows = connection.Query<RunDataDto>(sql);

            // Convert back from ms → TimeSpan
            res = rows.Select(r => r.ToDomain());
            connection.Close();
        }
        return res;
    }

    public RunData Insert(RunData runData)
    {
        Create(ref runData);
        return runData;
    }


    private record RunDataDto
    {
        public long Id { get; init; }
        public string PlayerName { get; init; } = null!;
        public string LevelName { get; init; } = null!;
        public int CollectedCoins { get; init; }
        public long TimeToFinishMs { get; init; } // stored in milliseconds

        public static RunDataDto FromDomain(RunData r) => new()
        {
            // doesn't matter as the create method doesn't pass it to the function call.
            Id = r.Id ?? -1,
            PlayerName = r.PlayerName,
            LevelName = r.LevelName,
            CollectedCoins = r.CollectedCoins,
            TimeToFinishMs = (long)r.TimeToFinish.TotalMilliseconds
        };

        public RunData ToDomain() => new
        (
            Id: Id,
            PlayerName: PlayerName,
            LevelName: LevelName,
            CollectedCoins: CollectedCoins,
            TimeToFinish: TimeSpan.FromMilliseconds(TimeToFinishMs)
        );
    }
}

internal class ConnectionFactory(string dbPath) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new SqliteConnection(dbPath);
}