using Godot;
using System;
using Brackeys.Knight.Util;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Brackeys.Knight.Coin;

public partial class Coin : Area2D
{
	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// </summary>
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	void OnBodyEntered(Node2D body)
	{
		Printer.Print($"{body.Name} entered {Name}");
		QueueFree();
		Printer.Print($"Queued {Name} for removal");
	}
}
