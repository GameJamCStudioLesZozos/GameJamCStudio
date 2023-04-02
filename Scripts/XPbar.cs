using Godot;
using System;

public partial class XPbar : Control
{
	[Export] public PlayerController player;

	[Export] private TextureProgressBar xpBarProgress;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		xpBarProgress.MaxValue = player.XPToNextLevel;
		xpBarProgress.Value = 0;
	}

	public void OnPlayerXPChanged(int newXP)
	{
		// GD.Print($"Updating XP to {newXP}");
		xpBarProgress.Value = newXP;
	}
	
	public void OnPlayerXPToNextLevelChanged(int newXPToNextLevel)
	{
		// GD.Print($"Updating XPToNextLevel to {newXPToNextLevel}");
		xpBarProgress.MaxValue = newXPToNextLevel;
	}
}
