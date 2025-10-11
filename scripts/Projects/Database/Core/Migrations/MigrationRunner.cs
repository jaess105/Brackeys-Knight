namespace Database.Core.Migrations;

using System;
using System.Reflection;
using DbUp;
using DbUp.Engine;

internal sealed class MigrationRunner(string connectionString, Action<string>? print = null)
{
    private readonly string _connectionString = connectionString;
    private readonly Action<string>? _print = print;

    public void RunMigrations()
    {

        UpgradeEngine upgrader = DeployChanges
            .To.SqliteDatabase(_connectionString)
            .WithScriptsEmbeddedInAssemblies([typeof(MigrationRunner).Assembly, Assembly.GetExecutingAssembly()])
            .LogToConsole()
            .Build();

        DatabaseUpgradeResult result = upgrader.PerformUpgrade();

        if (result.Successful)
        {
            _print?.Invoke("Database upgraded successfully.");
        }
        else
        {
            _print?.Invoke($"Database upgrade failed: {result.Error.Message}");
            throw new ArgumentException($"Database upgrade failed: {result.Error.Message}");
        }
    }
}


