using Brackeys.Knight.Util;
using Godot;

namespace Brackeys.Knight.Player;

public partial class Player : CharacterBody2D
{
	public const float Speed = 130.0f;
	public const float JumpVelocity = -300.0f;

	private AnimatedSprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
		_sprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (ExecuteYeeting((float)delta)) { return; }

		Vector2 velocity = this.ApplyGravity((float)delta, this.Velocity);

		Jump(ref velocity);

		Vector2 direction = GetDirection();
		Move(direction, ref velocity);
		ChooseAnimation(direction);

		Velocity = velocity;
		MoveAndSlide();
	}


	private void Jump(ref Vector2 velocity)
	{
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
	}

	private void Move(Vector2 direction, ref Vector2 velocity)
	{
		// sprite direction facing change
		if (direction == Vector2.Right)
		{
			_sprite.FlipH = false;
		}
		else if (direction == Vector2.Left)
		{
			_sprite.FlipH = true;
		}


		// movement towards what direction
		velocity.X = direction == Vector2.Zero
			? Mathf.MoveToward(Velocity.X, 0, Speed)
			: direction.X * Speed;
	}

	private void ChooseAnimation(Vector2 direction)
	{
		if (IsOnFloor())
		{
			_sprite.Play(direction == Vector2.Zero ? "idle" : "run");
			return;
		}

		_sprite.Play("jump");
	}

	private static Vector2 GetDirection()
	{
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		// ignore up and down.
		return (direction == Vector2.Up || direction == Vector2.Down) ? Vector2.Zero : direction;
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

		_sprite.Play("die");
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
