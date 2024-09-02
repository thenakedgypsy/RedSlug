using Godot;
using System;


public partial class Player : CharacterBody2D
{
	public float Speed = 80.0f;
	public float JumpVelocity = -200.0f;
	public Vector2 Direction;
	public AnimatedSprite2D Sprite;
	private Vector2 _previousPos;
	public int MaxJumps;
	public int JumpsRemaining;
	public bool IsDying;
	public string PrevAnimation;
	public bool IsFalling;
	public string PlayerForm;
	public bool IsJumping;
	private float _edgePushForce = 20f;

    public override void _Ready()
    {
		base._Ready();
		Initialize();
    }
	public void Initialize()
	{
		IsJumping = false;
		IsFalling = false;
		PrevAnimation = "idle";
		IsDying = false;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");       
		_previousPos = Position;
		MaxJumps = 1;
		JumpsRemaining = 1;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * 0.8f * (float)delta;
			IsFalling = true;
		}
		else
		{
			IsFalling = false;
		}

		if(!IsDying && StateManager.Instance.GetState() == "Gameplay")
			{
			// Handle Jump.
			if (Input.IsActionJustPressed("ui_accept") && JumpsRemaining > 0)
			{
				JumpsRemaining -= 1;
				IsJumping = true;
				velocity.Y = JumpVelocity;
				
				GD.Print($"Jumps Remaining: {JumpsRemaining}");
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (Direction != Vector2.Zero)
			{
				velocity.X = Direction.X * Speed;
				if (IsOnFloor() && !IsOnFloorOnly())
                {
                    velocity.X += Direction.X * _edgePushForce;
                }
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

			if(IsOnFloor())
			{
				if(IsJumping)
				{
					IsJumping = false;
				}
				else
				{
					JumpsRemaining = MaxJumps;
				}
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
			Sprite.Animation = "falling";
			PrevAnimation = "falling";
		}
		else if(Direction.X == 1 && IsOnFloor())
		{
			Sprite.Animation = "moveR";
			PrevAnimation = "moveR";
		}
		else if(Direction.X == -1 && IsOnFloor())
		{
			Sprite.Animation = "moveL";
			PrevAnimation ="moveL";
		}
		else
		{
			Sprite.Animation = "idle";
			PrevAnimation = "idle";
		}
		_previousPos = Position;
	}

	public void ResetBackground()
	{
		Node bgNode = GetTree().Root.FindChild("Background", true, false);
		Background bg = (Background)bgNode;
		bg.ResetBackgroundPosition();
	}
	
}
