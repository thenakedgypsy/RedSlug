using Godot;
using System;

public partial class StateManager : Node
{
	public static StateManager Instance;
	private String _state;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_state = "Gameplay";
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetState(string state)
	{
		if(state == "Gameplay")
		{
			_state = "Gameplay";
		}
		else if(state == "Evolution")
		{
			_state = "Evolution";
		}
		else if(state == "Cinematic")
		{
			_state = "Cinematic";
		}
		else
		{
			GD.Print($"State Machine Error: Attempt to set unrecognised state: {state}");
		}
	}

	public String GetState()
	{
		return _state;
	}
}
