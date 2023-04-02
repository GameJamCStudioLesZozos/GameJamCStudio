using Godot;
using System;

public partial class PlayerXPLabel : Label
{
	[Export] public PlayerController player;

	private int xp = 0;
	private int xpToNextLevel = 100;

	public override void _Ready()
	{
		xp = 0;
		xpToNextLevel = player.XPToNextLevel;
		UpdateText();
	}

    private void UpdateText()
    {
        Text = $"XP: {xp}/{xpToNextLevel}";
    }

    public void OnPlayerXPChanged(int newXP)
	{
		xp = newXP;
		UpdateText();
	}

	public void OnPlayerXPToNextLevelChanged(int newXPToNextLevel)
	{
		xpToNextLevel = newXPToNextLevel;
		UpdateText();
	}
}
