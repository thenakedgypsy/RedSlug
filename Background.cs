using Godot;
using System;

public partial class Background : ParallaxBackground
{
	[Export]
	public NodePath PlayerPath;
	private CharacterBody2D _slug;
	private Vector2 _lastPos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_slug = GetNode<CharacterBody2D>(PlayerPath);
		_slug = GetNode<CharacterBody2D>(PlayerPath);
        if (_slug != null)
        {
            _lastPos = _slug.GlobalPosition;
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
		Vector2 playerMovement = _slug.GlobalPosition - _lastPos;
        ScrollOffset += playerMovement;
        _lastPos = _slug.GlobalPosition;
	}

	public void SetupLayerMirroring(ParallaxLayer layer)
	{
		var sprite = layer.GetNode<Sprite2D>("Sprite2D");
		layer.MotionMirroring = sprite.Texture.GetSize() * sprite.Scale;
	}
}
