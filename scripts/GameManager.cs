using Brackeys.Knight.Coin;
using Brackeys.Knight.Util;
using Godot;
using System;

public partial class GameManager : Node
{
	private int _score;
	private int _maxScore;
	private Label _scoreLabel;

	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_maxScore = this.GetNodesOfType<Coin>().Count;
	}


	public void AddPoint()
	{
		_score++;
		_scoreLabel.Text = $"You collected {_score} / {_maxScore} coins!";
	}
}
