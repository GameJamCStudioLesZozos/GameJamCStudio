using Godot;
using System;

public partial class FoeLifebar : Control
{
	[Export] public Foe foe;

	[Export] private TextureProgressBar lifebarProgress;

	[Export] private Label DamageTakenText;

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
		if (DamageTakenText.SelfModulate.A > 0)
		{
			DamageTakenText.SelfModulate = new Color(DamageTakenText.SelfModulate, DamageTakenText.SelfModulate.A-0.005f);
		}
	}

	public void OnFoeHealthChanged(int newHealth)
	{
		GD.Print($"Updating Health to {newHealth}");
		if (lifebarProgress.Value - newHealth > 0)
		{
			DamageTakenText.Text = "-"+((int)(lifebarProgress.Value - newHealth)).ToString();
			DamageTakenText.SelfModulate = new Color(DamageTakenText.SelfModulate, 1);
		}
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
