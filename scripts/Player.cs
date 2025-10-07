using Brackeys.Knight.Util;
using Godot;
using System;

namespace Brackeys.Knight.Player;

public partial class Player : CharacterBody2D
{
	public const float Speed = 130.0f;
	public const float JumpVelocity = -300.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = this.ApplyGravity((float)delta, this.Velocity);

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		velocity.X = direction == Vector2.Zero
			? Mathf.MoveToward(Velocity.X, 0, Speed)
			: direction.X * Speed;

		Velocity = velocity;
		MoveAndSlide();
	}
}
