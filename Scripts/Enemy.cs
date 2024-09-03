using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public string EnemyName;
	private AnimatedSprite2D _sprite;
	private Player _player;
	private float _currentTime;
	private float _previousAttackTime;
	private bool _playerInAttackZone;
	private bool _facingLeft;
	private bool _killedPlayer;
	private AudioStreamPlayer2D _deathSound;
	
	[Export]
	public float AttackDelay = 2.5f;

    public override void _Ready()
    {
		InitAudio();
		_killedPlayer = false;
		_playerInAttackZone = false;
		_previousAttackTime = 0f;
		base._Ready();
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
    }

	public void InitAudio()
	{
		_deathSound = GetNode<AudioStreamPlayer2D>("Death");
		AudioStreamOggVorbis deathAudio;
		try
		{
			deathAudio = (AudioStreamOggVorbis)ResourceLoader.Load<AudioStream>($"res://Assets/Audio/{EnemyName}Death.ogg");
		}
		catch
		{
			GD.Print($"Failed to load audio: res://Assets/Audio/{EnemyName}Death.ogg, loaded bloop instead");
			deathAudio = (AudioStreamOggVorbis)ResourceLoader.Load<AudioStream>($"res://Assets/Audio/Bloop.ogg");
			
		}
		_deathSound.Stream = deathAudio;
	}
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		_player = GetPlayer();
		_currentTime += (float)delta;
		Attack();
		FacePlayer();
	}

	public void TopColliderEntered(Node2D body)
	{
		if(body is Player && _sprite.Animation != "death" && !_killedPlayer)
		{
			_sprite.Animation = "death";
			_deathSound.Play();
			_player.Bounce();
		}
	}

	public void TopAttackColliderEntered(Node2D body)
	{
		if(body is Player && _sprite.Animation != "death" && !_killedPlayer)
		{
			if(_sprite.Animation == "attack")
			{
				_sprite.Animation = "death";
				_deathSound.Play();
				_player.Bounce();
			}
		}
	}

	public void DangerZoneDefaultEntered(Node2D body)
	{
		if(body is Player && _sprite.Animation != "death")
		{
			_player.Death();
		}
	}

	public void DangerZoneAttackEntered(Node2D body)
	{
		if(body is Player)
		{
			_playerInAttackZone = true;
		}
	}

	public void DangerZoneAttackExit(Node2D body)
	{
		if(body is Player)
		{
			_playerInAttackZone = false;
		}
	}
	public void Attack()
	{
		
		if(_currentTime - _previousAttackTime >= AttackDelay && _sprite.Animation == "idle")
		{			
			_sprite.Animation = "attack";
			_sprite.Play();
			if(_playerInAttackZone)
			{
				_killedPlayer = true;
				_player.Death();
			}
			_previousAttackTime = _currentTime;
		}
	}

	public void FacePlayer()
	{
		if(_sprite.Animation == "idle")
		{
			if(_player.Position.X - Position.X <= 0 && !_facingLeft)
			{
				Scale = new Vector2(1, 1);
				_facingLeft = true;
			}
			if(_player.Position.X - Position.X > 0 && _facingLeft)
			{
				Scale = new Vector2(Scale.X * -1, 1);
				_facingLeft = false;
			}
		}
	}
		
	public void AnimationFinished()
	{
		string anim = _sprite.Animation;
		if(anim == "death")
		{
			Death();
		}
		if(anim == "attack")
		{
			_sprite.Animation = "idle";
			_sprite.Play();
		}
	}

	public void Death()
	{
		QueueFree();
	}

	public Player GetPlayer()
	{
	  
	    var playerNode = GetTree().Root.FindChild("Player", true, false);
	    if (playerNode == null)
	    {
	        GD.Print("Player node not found in the scene tree");
	        return null;
	    }
	    else
	    {
			//GD.Print($"Returning player to enemy");
	        return (Player)playerNode;
	    }
	}
}
