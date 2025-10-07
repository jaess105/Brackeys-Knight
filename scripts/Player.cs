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
		if (ExecuteYeeting((float)delta)) { return; }


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

/// <summary>
/// Implement and encapsulate the yeeting functionality
/// </summary>
public partial class Player : IYeetable
{
	private float _rotationSpeed = 0f;
	private bool _isDead = false;

	public void Yeet(Vector2 impulse)
	{
		_isDead = true;
		_rotationSpeed = Mathf.DegToRad(360); // one full spin per second

		Velocity = impulse;

		GetNode<CollisionShape2D>(nameof(CollisionShape2D)).QueueFree();
	}

	private bool ExecuteYeeting(float delta)
	{
		if (!_isDead) { return false; }

		Velocity += Vector2.Down * GetGravity() * delta;
		Rotation += _rotationSpeed * delta;

		MoveAndSlide();
		return true;
	}
}

