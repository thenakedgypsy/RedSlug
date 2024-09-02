using Godot;
using System;


public partial class Egg : Player
{

	private int _sway1;
	private int _sway2;
	private bool _swayedRight;
	private bool _swayedLeft;
	private bool _hasHatched;
    public override void _Ready()
    {
		_hasHatched = false;
		_swayedRight = false;
		_sway1 = 0;
		_sway2 = 0;
		PlayerForm = "Egg";
		this.Speed = 0f;
		this.JumpVelocity = 0f;
        base._Ready();
		base.Initialize();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		SwayCounter();
		if(!_hasHatched)
		CheckHatch();
    }

    public void AnimationFinished()
	{
		if(Sprite.Animation == "hatch")
		{
			GD.Print($"hatched!");
			Evolve();
		}
	}

	public void SwayCounter()
	{
		if(Direction.X == 1f && !_swayedLeft)
		{
			_sway1 += 1;
			GD.Print($"Sway1 = {_sway1}");
			_swayedLeft = true;
			_swayedRight = false;
		}
		if(Direction.X == -1f && !_swayedRight)
		{
			_sway2 += 1;
			GD.Print($"Sway2 = {_sway2}");
			_swayedLeft = false;
			_swayedRight = true;
		}
	}

	public void CheckHatch()
	{
		if(_sway1 > 4 || _sway2 >4)
		{
			Hatch();
		}
	}

	public void Hatch()
	{
		_hasHatched = true;
		StateManager.Instance.SetState("Cinematic");
		Sprite.Animation = "hatch";
		Sprite.Play();
	}
	
	public void Evolve()
	{
		PackedScene redSlug = ResourceLoader.Load<PackedScene>("res://Prefabs/RedSlug.tscn");
		Node slugInstance = redSlug.Instantiate();
		GetParent().AddChild(slugInstance);
		if (slugInstance is Node2D node2D)
        {
                node2D.Position = new Vector2(31, 291);
				Name = "oldPlayer";
				node2D.Name = "Player";
				
        }

		PackedScene oldEggScene = ResourceLoader.Load<PackedScene>("res://Prefabs/HatchedEgg.tscn");
		Node oldEgg = oldEggScene.Instantiate();
		GetParent().AddChild(oldEgg);
		if (oldEgg is Node2D eggNode)
        {
                eggNode.Position = new Vector2(9, 275);
        }
		StateManager.Instance.SetState("Gameplay");
		QueueFree();
		ResetBackground();
		
	}


}
