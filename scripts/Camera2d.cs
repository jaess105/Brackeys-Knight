using Brackeys.Knight.Player;
using Godot;
using System;

public partial class Camera2d : Camera2D
{
	private bool _followPlayer = true;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (_followPlayer)
		{
			Position = GetNode<Player>("%Player").Position;
		}
	}

	public void FollowPlayer() => _followPlayer = true;
	public void StopFollowPlayer() => _followPlayer = false;
}
