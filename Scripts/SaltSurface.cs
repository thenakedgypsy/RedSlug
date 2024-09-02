using Godot;
using System;

public partial class SaltSurface : Area2D
{
	[Signal]
	public delegate void TouchingSaltSurfaceEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConnectToPlayer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TouchingSalt(Node2D body)
	{
		if(body is RedSlug)
		{
			EmitSignal(nameof(TouchingSaltSurface));
			GD.Print("Emitting touching salt");
		}
	}

	public void ConnectToPlayer()
	{
		var playerGroup = GetTree().GetNodesInGroup("Player");
		if(playerGroup.Count > 0)
		{
			var player = playerGroup[0] as RedSlug;//potentially needs to iterate through
			if(player != null)						//if we are changing slug
			{
				GD.Print("Linking slug to salts");
				TouchingSaltSurface += player.Salt;				
			}
			else
			{
				GD.Print("Player group item found but is not type RedSlug");
			}
		}
		else
		{
			GD.Print("Player not found");
		}
	}
}
