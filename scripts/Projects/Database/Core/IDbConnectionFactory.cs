using System.Data;

namespace Database.Core;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public interface IAsyncDbConnectionFactory : IDbConnectionFactory
{
    IDbConnection IDbConnectionFactory.CreateConnection()
    {
        var connectionTask = CreateConnectionAsync();
        connectionTask.Wait();
        return connectionTask.Result;
    }

    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}
