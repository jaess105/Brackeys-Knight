
namespace Database;

public record class RunData
{
    public long? Id { get; set; }
    public required string PlayerName { get; init; }
    public required int CollectedCoins { get; init; }
    public required TimeSpan TimeToFinish { get; init; }
}