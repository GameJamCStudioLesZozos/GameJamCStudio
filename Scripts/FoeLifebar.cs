using Godot;
using System;

public partial class FoeLifebar : Control
{
	[Export] public Foe foe;

	[Export] private TextureProgressBar lifebarProgress;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lifebarProgress.MaxValue = foe.MaxHealth;
		lifebarProgress.Value = foe.Health;
		lifebarProgress.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnFoeHealthChanged(int newHealth)
	{
		GD.Print($"Updating Health to {newHealth}");
		lifebarProgress.Value = newHealth;
		if (lifebarProgress.Value != lifebarProgress.MaxValue)
		{
			lifebarProgress.Visible = true;
		}
	}

	public void OnFoeMaxHealthChanged(int newMaxHealth)
	{
		GD.Print($"Updating Health to {newMaxHealth}");
		lifebarProgress.MaxValue = newMaxHealth;
	}
}
