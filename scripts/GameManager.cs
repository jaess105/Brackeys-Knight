using Brackeys.Knight.Coin;
using Brackeys.Knight.Hud;
using Brackeys.Knight.Util;
using Godot;
using System;

public partial class GameManager : Node
{
	[Signal]
	public delegate void PointAddedEventHandler(int score);
	[Signal]
	public delegate void PlayerDiedEventHandler(int finalScore);

	private int _score = 0;
	private int _maxScore;
	private Label ScoreLabel => GetNode<Label>("ScoreLabel");
	private Hud Hud => GetNode<Hud>("%HUD");

	public override void _Ready()
	{
		_maxScore = this.GetNodesOfType<Coin>().Count;

		SetScoreText();
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
		EmitSignal(SignalName.PlayerDied, _score);
	}

	public void OnRestart()
	{
		this.GameOver();	
	}
}
