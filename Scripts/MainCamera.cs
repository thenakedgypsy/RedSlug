using Godot;
using System;

public partial class MainCamera : Camera2D
{
	private float _smoothing = 0.05f;
	private float _verticalOffset = 110f;
	private float _smoothingThreshold = 10f;
	private Node _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Follow();
	}

	public Node GetPlayer()
	{
	  
	    var playerNode = GetTree().Root.FindChild("Player", true, false);
	    if (playerNode == null)
	    {
	        GD.Print("Player node not found in the scene tree");
	        return null;
	    }
	    else
	    {
			//GD.Print($"Returning path: {playerNode.GetPath()}");
	        return playerNode;
	    }
	}

		public void Follow()
	{
		Node node = GetPlayer();
		Node2D node2d = (Node2D)node;
		Vector2 targetPosition = node2d.GlobalPosition;
		targetPosition.Y -= _verticalOffset;
        Vector2 currentPosition = GlobalPosition;
        
		float distance = currentPosition.DistanceTo(targetPosition);

        if (distance > _smoothingThreshold)
        {
            // Apply smoothing for long distances
            Vector2 newPosition = currentPosition + (targetPosition - currentPosition) * _smoothing;
            GlobalPosition = newPosition;
        }
        else
        {
            // Exact positioning for short distances
            GlobalPosition = targetPosition;
        }
	}
}
