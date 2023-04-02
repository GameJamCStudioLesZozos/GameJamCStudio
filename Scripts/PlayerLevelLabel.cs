using Godot;
using System;

public partial class PlayerLevelLabel : Label
{
	[Export] public PlayerController player;

	private int level = 0;

	public override void _Ready()
	{
		level = 1;
		UpdateText();
	}

    private void UpdateText()
    {
        Text = $"Level: {level}";
    }

    public void OnPlayerLevelChanged(int newLevel)
	{
		level = newLevel;
		UpdateText();
	}
}
