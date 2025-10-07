using Brackeys.Knight.Util;
using Godot;
using System;


namespace Brackeys.Knight.Enemy;

public partial class EnemyGreen : CharacterBody2D
{
	public const float Speed = 60;

	private Vector2 _direction;
	private RayCast2D _rayCastRight;
	private RayCast2D _rayCastLeft;
	private RayCast2D _rayCastBottom;
	private AnimatedSprite2D _sprite;

	public override void _Ready()
	{
		_rayCastRight = GetNode<RayCast2D>("RayCastRight");
		_rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		_rayCastBottom = GetNode<RayCast2D>("RayCastBottom");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		GoRight();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_rayCastBottom.IsColliding())
		{
			if (_direction == Vector2.Left) { GoRight(); } else { GoLeft(); }
		}
		else if (_rayCastRight.IsColliding())
		{
			GoLeft();
		}
		else if (_rayCastLeft.IsColliding())
		{
			GoRight();
		}

		Vector2 velocity = Velocity;

		// // Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			velocity.X = _direction == Vector2.Zero
				? Mathf.MoveToward(Velocity.X, 0, Speed)
				: _direction.X * Speed;
		}

		Velocity = velocity;
		MoveAndSlide();
	}


	private void GoLeft()
	{
		_direction = Vector2.Left;
		_sprite.FlipH = true;
	}

	private void GoRight()
	{
		_direction = Vector2.Right;
		_sprite.FlipH = false;
	}
}
