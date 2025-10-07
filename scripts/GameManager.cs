using Brackeys.Knight.Coin;
using Brackeys.Knight.Util;
using Godot;
using System;

public partial class GameManager : Node
{
	private int _score = 0;
	private int _maxScore;
	private Label _scoreLabel;

	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_maxScore = this.GetNodesOfType<Coin>().Count;

		SetScoreText();
	}


	public void AddPoint()
	{
		_score++;
		SetScoreText();
	}

	private void SetScoreText() => _scoreLabel.Text = $"You collected {_score} / {_maxScore} coins!";
}
