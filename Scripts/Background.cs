using Godot;
using System;

public partial class Background : ParallaxBackground
{
	private CharacterBody2D _player;
	private Vector2 _lastPos;
	private Vector2 _targetScrollOffset;
	private float _lerpSpeed = 15f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		GetPlayer();
        if (_player != null)
        {
            _lastPos = _player.GlobalPosition;
        }
        else
        {
            GD.PrintErr("Player node not found!");
        }

		foreach(var child in GetChildren())
		{
			if(child is ParallaxLayer layer)
			{
				SetupLayerMirroring(layer);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetPlayer();
		Vector2 playerMovement = _player.GlobalPosition - _lastPos;
		_targetScrollOffset += playerMovement;
        ScrollOffset = ScrollOffset.Lerp(_targetScrollOffset, (float)delta * _lerpSpeed);
        _lastPos = _player.GlobalPosition;

	}

	public void SetupLayerMirroring(ParallaxLayer layer)
	{
		var sprite = layer.GetNode<Sprite2D>("Sprite2D");
		layer.MotionMirroring = sprite.Texture.GetSize() * sprite.Scale;
	}

	public void GetPlayer()
	{
	  
	    var playerNode = GetTree().Root.FindChild("Player", true, false);
	    if (playerNode == null)
	    {
	        GD.Print("Player node not found in the scene tree");
	        return;
	    }
	    else
	    {
			//GD.Print($"Returning path: {playerNode.GetPath()}");
	        _player = GetNode<CharacterBody2D>(playerNode.GetPath());
	    }
	}

	public void ResetBackgroundPosition()
    {
        if (_player != null)
        {
			GetPlayer();
            _lastPos = _player.GlobalPosition;
            _targetScrollOffset = ScrollOffset;
        }
    }


}
