using Godot;
using System;

public partial class SaltSurface : Area2D
{
	private bool _connectedToSlug;

	[Signal]
	public delegate void TouchingSaltSurfaceEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ConnectToSlug();
	}

	public void TouchingSalt(Node2D body)
	{
		if(body is RedSlug)
		{
			EmitSignal(nameof(TouchingSaltSurface));
			GD.Print("Emitting touching salt");
		}
	}

	public void ConnectToSlug()
	{
	    if (_connectedToSlug)
	        return;

	    var playerNode = GetTree().Root.FindChild("Player", true, false);
	    if (playerNode == null)
	    {
	        GD.Print("Player node not found in the scene tree");
	        return;
	    }

	    if (playerNode is RedSlug slug)
	    {
	        GD.Print("Linking slug to salts");
	        TouchingSaltSurface += slug.Salt;
	        _connectedToSlug = true;
	    }
	    else
	    {
	       // GD.Print("Waiting for player to be RedSlug");
	    }
	}
}
