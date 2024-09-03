using Godot;
using System;


public partial class RedSlug : Player
{
    public override void _Ready()
    {
		
		PlayerForm = "RedSlug";
		this.Speed = 80.0f;
		this.JumpVelocity = -200.0f;
        base._Ready();
    }
    
	public void Salt()
	{
		base.Death();
		GD.Print("SALT + SLUG = DED");
		IsDying = true;
		if(IsFalling)
		{
			Sprite.Animation = "melt";
		}
		else if(PrevAnimation == "moveR")
		{
			Sprite.Animation = "meltR";
		}
		else if(PrevAnimation == "moveL")
		{
			Sprite.Animation = "meltL";
		}
		else
		{
			Sprite.Animation = "melt";
		}		
	}

	public override void Death()
	{		
		Salt();
	}

	public void AnimationFinished()
	{
		if(Sprite.Animation == "meltR" || 
			Sprite.Animation =="meltL" || 
			Sprite.Animation == "melt")
		{
			GetTree().ReloadCurrentScene();
		}
	}

	
}
