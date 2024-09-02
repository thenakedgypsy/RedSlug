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
	private bool _isDying;
	private string _prevAnimation;
	private bool _isFalling;

    public override void _Ready()
    {
		_isFalling = false;
		_prevAnimation = "idle";
		_isDying = false;
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
			_isFalling = true;
		}
		else
		{
			_isFalling = false;
		}

		if(!_isDying && StateManager.Instance.GetState() == "Gameplay")
			{
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
				//GD.Print($"{_direction}");
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
	}

	public void AdjustAnimation()
	{

		Vector2 currentPosition = Position;
		if(currentPosition.Y != _previousPos.Y)
		{
			_slugSprite.Animation = "falling";
			_prevAnimation = "falling";
		}
		else if(_direction.X == 1 && IsOnFloor())
		{
			_slugSprite.Animation = "moveR";
			_prevAnimation = "moveR";
		}
		else if(_direction.X == -1 && IsOnFloor())
		{
			_slugSprite.Animation = "moveL";
			_prevAnimation ="moveL";
		}
		else
		{
			_slugSprite.Animation = "idle";
			_prevAnimation = "idle";
		}
		_previousPos = Position;
	}

	public void Salt()
	{
		GD.Print("SALT");
		_isDying = true;
		if(_isFalling)
		{
			_slugSprite.Animation = "melt";
		}
		else if(_prevAnimation == "moveR")
		{
			_slugSprite.Animation = "meltR";
		}
		else if(_prevAnimation == "moveL")
		{
			_slugSprite.Animation = "meltL";
		}
		else
		{
			_slugSprite.Animation = "melt";
		}		
	}

	public void AnimationFinished()
	{
		if(_slugSprite.Animation == "meltR" || 
			_slugSprite.Animation =="meltL" || 
			_slugSprite.Animation == "melt")
		{
			GetTree().ReloadCurrentScene();
		}
	}

	
}
