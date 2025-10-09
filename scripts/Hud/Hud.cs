using Godot;
using System;

namespace Brackeys.Knight.Hud;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	[Signal]
	public delegate void RestartGameEventHandler();

	private Label Message => GetNode<Label>("Message");
	private Timer MessageTimer => GetNode<Timer>("MessageTimer");

	private Button StartButton => GetNode<Button>("StartButton");
	private Button RestartButton => GetNode<Button>("RestartButton");

	private Label ScoreLabel => GetNode<Label>("HBoxContainer/ScoreLabel");

	private void ShowMessage(string text)
	{
		var message = Message;
		message.Text = text;
		message.Show();
	}

	public async void ShowGameOver(int finalScore)
	{
		var messageTimer = MessageTimer;
		ShowMessage($"Game Over");

		messageTimer.Start();
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		ShowMessage("Try again!");

		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		RestartButton.Show();
	}

	public void UpdateScore(int score)
	{
		ScoreLabel.Text = score.ToString();
	}

	private void OnStartButtonPressed()
	{
		StartButton.Hide();
		Message.Hide();

		EmitSignal(SignalName.StartGame);
	}

	private void OnRestartButtonPressed()
	{
		RestartButton.Hide();
		Message.Hide();

		EmitSignal(SignalName.RestartGame);
	}

	private void OnMessageTimerTimeout()
	{
		Message.Hide();
	}
}
