using Brackeys.Knight.Coin;
using Database;
using Brackeys.Knight.Hud;
using Brackeys.Knight.Util;
using Godot;
using Brackeys.Knight.Player;
using Brackeys.Knight.Interfaces;

public partial class GameManager : Node, IPlayerTimerTracker
{
	[Signal]
	public delegate void PointAddedEventHandler(int score);
	[Signal]
	public delegate void PlayerDiedEventHandler(int finalScore);

	private int _score = 0;
	private int _maxScore;
	private Label ScoreLabel => GetNode<Label>("ScoreLabel");
	private Hud Hud => GetNode<Hud>("%HUD");
	private PlayerRunTimer RunTimer => GetNode<PlayerRunTimer>("PlayerRunTimer");

    public ulong ElapsedTime => RunTimer.ElapsedTime;

    private readonly IRunDataRepository _runDataRepository = MainRepository.Instance;

	public override void _Ready()
	{
		string dbPath = ProjectSettings.GlobalizePath("user://game_data.db");
#if DEBUG
		try
		{
			if (File.Exists(dbPath))
			{
				// Delete the database file before upgrade
				File.Delete(dbPath);
				Printer.Print("Debug mode: deleted existing database.");
			}
		}
		catch { }
#endif
		MainRepository.Init(dbPath);

		_maxScore = this.GetNodesOfType<Coin>().Count;

		SetScoreText();

		Printer.Print(string.Join("\n", _runDataRepository.GetPreviousRunData()));
	}


	public void AddPoint()
	{
		_score++;

		SetScoreText();
		EmitSignal(SignalName.PointAdded, _score);
	}

	private void SetScoreText() => ScoreLabel.Text = $"You collected {_score} / {_maxScore} coins!";

	public void OnPlayerDeath()
	{
		RunTimer.Stop();
		EmitSignal(SignalName.PlayerDied, _score);
	}

	public void OnStart() => RunTimer.Start();

	public void OnPlayerWon(string levelName)
	{
		var timer = RunTimer;
		timer.Stop();

		RunData runData = new(null, "Jannik", levelName, _score, TimeSpan.FromMilliseconds((long)timer.FinalElapsedTime));
		Printer.Print($"Took you long enough {runData}");
		Printer.Print($"{timer.FinalElapsedTime}ms");

		_runDataRepository.Insert(runData);
	}

	public void OnRestart()
	{
		RunTimer.Reset();
		this.GameOver();
	}
}
