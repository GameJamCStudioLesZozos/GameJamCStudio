using Godot;
using System;

public partial class Lifebar : Control
{
	[Export] public PlayerController player;

	[Export] private TextureProgressBar lifebarProgress;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lifebarProgress.MaxValue = player.MaxHealth;
		lifebarProgress.Value = player.Health;
	}

	public void OnPlayerHealthChanged(int newHealth)
	{
		// GD.Print($"Updating Health to {newHealth}");
		lifebarProgress.Value = newHealth;
	}

	public void OnPlayerMaxHealthChanged(int newMaxHealth)
	{
		// GD.Print($"Updating MaxHealth to {newMaxHealth}");
		lifebarProgress.MaxValue = newMaxHealth;
	}
}
