using Brackeys.Knight.Interfaces;
using Brackeys.Knight.Util;
using Godot;

namespace Brackeys.Knight.Killzone;

public partial class Killzone : Area2D
{
	public void OnBodyEntered(Node2D body)
	{
		var timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += () =>
		{
			Engine.TimeScale = 1;
			QueueFree();
		};


		if (body is IYeetable player)
		{
			Printer.Print("You died!");
			Engine.TimeScale = .5;
			player.Yeet(new Vector2(200, -400));
			timer.Start();
		}
#if DEBUG
		else
		{
			Printer.Print("An object that was not yeetable entered the killzone");
		}
#endif

	}
}
