
namespace Database;

public record RunData(
    long? Id,
    string PlayerName,
    string LevelName,
    int CollectedCoins,
    TimeSpan TimeToFinish
);