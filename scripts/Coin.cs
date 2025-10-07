using Godot;
using System;
using Brackeys.Knight.Util;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Brackeys.Knight.Coin;

public partial class Coin : Area2D
{
	private GameManager _gameManager;

	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("%GameManager");

		BodyEntered += OnBodyEntered;
	}

	void OnBodyEntered(Node2D body)
	{
		_gameManager.AddPoint();
		QueueFree();
	}
}
