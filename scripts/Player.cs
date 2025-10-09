using System;
using System.Collections.Generic;
using Brackeys.Knight.Util;
using Godot;

namespace Brackeys.Knight.Player;

public partial class Player : CharacterBody2D
{
	public const float Speed = 130.0f;
	public const float JumpVelocity = -300.0f;

	private AnimatedSprite2D _sprite;

	private IPlayerState playerState;

	private readonly StateContainer<IPlayerState> _stateContainer = new();

	public Player()
	{
		_stateContainer.AddState(new NormalPlayerState(this));
		_stateContainer.AddState(new DisabledPlayerState());
		_stateContainer.AddState(new YeetingPlayerState(this));

		playerState = _stateContainer.Get<NormalPlayerState>();
	}

	public override void _Ready()
	{
		DisablePlayer();
		_sprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
		_sprite.Play("idle");
	}

	public void EnablePlayer() => playerState = _stateContainer.Get<NormalPlayerState>();
	public void DisablePlayer() => playerState = _stateContainer.Get<DisabledPlayerState>();


	public override void _PhysicsProcess(double delta)
	{
		playerState._PhysicsProcess(delta);
	}

	private interface IPlayerState
	{
		void _PhysicsProcess(double delta);
	}

	private class DisabledPlayerState : IPlayerState
	{
		public void _PhysicsProcess(double delta) { }
	}

	private class NormalPlayerState(Player player) : IPlayerState
	{
		public void _PhysicsProcess(double delta)
		{
			Vector2 velocity = player.ApplyGravity((float)delta, player.Velocity);

			Jump(ref velocity);

			Vector2 direction = GetDirection();
			Move(direction, ref velocity);
			ChooseAnimation(direction);

			player.Velocity = velocity;
			player.MoveAndSlide();
		}


		private void Jump(ref Vector2 velocity)
		{
			if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}
		}

		private void Move(Vector2 direction, ref Vector2 velocity)
		{
			// sprite direction facing change
			if (direction == Vector2.Right)
			{
				player._sprite.FlipH = false;
			}
			else if (direction == Vector2.Left)
			{
				player._sprite.FlipH = true;
			}


			// movement towards what direction
			velocity.X = direction == Vector2.Zero
				? Mathf.MoveToward(velocity.X, 0, Speed)
				: direction.X * Speed;
		}

		private void ChooseAnimation(Vector2 direction)
		{
			if (player.IsOnFloor())
			{
				player._sprite.Play(direction == Vector2.Zero ? "idle" : "run");
				return;
			}

			player._sprite.Play("jump");
		}

		private static Vector2 GetDirection()
		{
			// Get the input direction and handle the movement/deceleration.
			Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			// ignore up and down.
			return (direction == Vector2.Up || direction == Vector2.Down) ? Vector2.Zero : direction;
		}
	}
}

/// <summary>
/// Implement and encapsulate the yeeting functionality
/// </summary>
public partial class Player : IYeetable
{
	[Signal]
	public delegate void PlayerDiedEventHandler();

	private float _rotationSpeed = 0f;

	public async void Yeet(Vector2 impulse)
	{
		EmitSignal(SignalName.PlayerDied);

		playerState = _stateContainer.Get<YeetingPlayerState>();
		_rotationSpeed = Mathf.DegToRad(360); // one full spin per second

		Velocity = impulse;

		GetNode<CollisionShape2D>(nameof(CollisionShape2D)).QueueFree();

		_sprite.Play("die");

		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
		DisablePlayer();
	}

	private class YeetingPlayerState(Player player) : IPlayerState
	{
		public void _PhysicsProcess(double delta)
		{
			player.Velocity += Vector2.Down * player.GetGravity() * (float)delta;
			player.Rotation += player._rotationSpeed * (float)delta;

			player.MoveAndSlide();
		}
	}
}

public class StateContainer<T>
{
	private readonly Dictionary<Type, T> dict = [];

	public void AddState<TI>(TI instance) where TI : class, T
		=> dict[typeof(TI)] = instance;

	public TI Get<TI>() where TI : class, T => (TI)dict[typeof(TI)];
}