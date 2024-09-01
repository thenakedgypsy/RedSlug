using Godot;
using System;


public partial class RedSlug : CharacterBody2D
{
	public const float Speed = 80.0f;
	public const float JumpVelocity = -200.0f;
	private Vector2 _direction;
	private AnimatedSprite2D _slugSprite;
	private Vector2 _previousPos;
	private int _maxJumps;
	private int _jumpsRemaining;

    public override void _Ready()
    {
		_slugSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        base._Ready();
		_previousPos = Position;
		_maxJumps = 2;
		_jumpsRemaining = 2;
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * 0.8f * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && _jumpsRemaining > 0)
		{
			velocity.Y = JumpVelocity;
			_jumpsRemaining =- 1;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		_direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (_direction != Vector2.Zero)
		{
			velocity.X = _direction.X * Speed;
			GD.Print($"{_direction}");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		if(IsOnFloor())
		{
			_jumpsRemaining = _maxJumps;
		}

		Velocity = velocity;
		MoveAndSlide();
		AdjustAnimation();
	}

	public void AdjustAnimation()
	{
		Vector2 currentPosition = Position;
		if(currentPosition.Y != _previousPos.Y)
		{
			_slugSprite.Animation = "falling";
		}
		else if(_direction.X == 1)
		{
			_slugSprite.Animation = "moveR";
		}
		else if(_direction.X == - 1)
		{
			_slugSprite.Animation = "moveL";
		}
		else
		{
			_slugSprite.Animation = "idle";
		}
		_previousPos = Position;

	}

	
}
