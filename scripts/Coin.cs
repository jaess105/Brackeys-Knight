using Godot;

namespace Brackeys.Knight.Coin;

public partial class Coin : Area2D
{
	private GameManager _gameManager;
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("%GameManager");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		BodyEntered += OnBodyEntered;
	}

	void OnBodyEntered(Node2D body)
	{
		_gameManager.AddPoint();
		_animationPlayer.Play("pickup");
	}
}
