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
            Create(new RunData { PlayerName = "Jannik", CollectedCoins = 1, TimeToFinish = TimeSpan.FromSeconds(240) });
        }
        return GetAll();
    }


    public void Create(RunData run)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        var sql = @"
            INSERT INTO RunData (PlayerName, CollectedCoins, TimeToFinish)
            VALUES (@PlayerName, @CollectedCoins, @TimeToFinishMs);
            SELECT last_insert_rowid();
        ";

        // Convert TimeSpan → milliseconds for SQLite
        var id = connection.ExecuteScalar<long>(sql, new
        {
            run.PlayerName,
            run.CollectedCoins,
            TimeToFinishMs = (long)run.TimeToFinish.TotalMilliseconds
        });

        run.Id = id;
    }

    public IEnumerable<RunData> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        var sql = "SELECT Id, PlayerName, CollectedCoins, TimeToFinish FROM RunData;";
        var rows = connection.Query<RunDataDto>(sql);

        // Convert back from ms → TimeSpan
        return rows.Select(r => r.ToDomain());
    }

    private record RunDataDto
    {
        public long Id { get; init; }
        public string PlayerName { get; init; } = null!;
        public int CollectedCoins { get; init; }
        public long TimeToFinish { get; init; } // stored in milliseconds

        public static RunDataDto FromDomain(RunData r) => new()
        {
            // doesn't matter as the create method doesn't pass it to the function call.
            Id = r.Id ?? -1,
            PlayerName = r.PlayerName,
            CollectedCoins = r.CollectedCoins,
            TimeToFinish = (long)r.TimeToFinish.TotalMilliseconds
        };

        public RunData ToDomain() => new()
        {
            Id = Id,
            PlayerName = PlayerName,
            CollectedCoins = CollectedCoins,
            TimeToFinish = TimeSpan.FromMilliseconds(TimeToFinish)
        };
    }
}

internal class ConnectionFactory(string dbPath) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new SqliteConnection(dbPath);
}