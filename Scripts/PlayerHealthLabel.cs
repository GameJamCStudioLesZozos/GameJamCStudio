using Godot;
using System;

public partial class PlayerHealthLabel : Label
{
	[Export] public PlayerController player;

	private int health = 0;
	private int maxHealth = 100;

	public override void _Ready()
	{
		health = player.Health;
		maxHealth = player.MaxHealth;
		UpdateText();
	}

    private void UpdateText()
    {
        Text = $"HP: {health}/{maxHealth}";
    }

    public void OnPlayerHealthChanged(int newHealth)
	{
		health = newHealth;
		UpdateText();
	}

	public void OnPlayerMaxHealthChanged(int newMaxHealth)
	{
		maxHealth = newMaxHealth;
		UpdateText();
	}
}
