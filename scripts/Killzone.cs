using Brackeys.Knight.Util;
using Godot;
using System;

namespace Brackeys.Knight.Killzone;

public partial class Killzone : Area2D
{
	public override void _Ready()
	{
		BodyEntered += (body) =>
		{
			var timer = GetNode<Timer>("Timer");
			timer.Timeout += () =>
			{
				GetTree().ReloadCurrentScene();
				Free();
			};

			timer.Start();
		};

	}
}
