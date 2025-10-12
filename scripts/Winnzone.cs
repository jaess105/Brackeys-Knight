using Brackeys.Knight.Player;
using Brackeys.Knight.Util;
using Godot;
using System;

public partial class Winnzone : Area2D
{
	[Signal]
	public delegate void PlayerWonLevelEventHandler(string levelName);

	public void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			Printer.Print("You Won!");
			EmitSignalPlayerWonLevel("Level1");
		}
#if DEBUG
		else
		{
			Printer.Print("An object that was not the player entered the winzone");
		}
#endif
	}
}
