using Godot;
using System;

public partial class Foe : AnimatableBody2D
{
	[Export] public float Speed = 500;
	[Export] public double StonksCooldown = 0.5f;
	public double CurrentStonksCooldown = 0.0f;
	[Export] public double DamageCooldown = 0.5f;
	public double CurrentDamageCooldown = 0.0f;
	
	[Export] public int Health = 10;
	[Export] public int MaxHealth = 10;

	public bool IsDead => Health <= 0;

	[Export] public int Damage = 10;
	[Export] public float DamageScaling = 0.5f;
	[Export] public float HealthScaling = 0.5f;
	[Export] public float SizeScaling = 1.2f;

	public Node2D player;
	private Vector2 direction;
	
	[Export] public Type type;

	[Signal] public delegate void HealthChangedEventHandler(int newHealth);
	[Signal] public delegate void MaxHealthChangedEventHandler(int newMaxHealth);
	[Signal] public delegate void DiedEventHandler(int newHealth);

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Node2D/Player");
		direction = (player.Position - this.Position).Normalized();
		CurrentStonksCooldown = 0.0f;
		Health = MaxHealth;
	}

	void Hit(Type typeOfBall, int ballPower)
	{
		// GD.Print($"Got hit by ball of type {typeOfBall} and power {ballPower}!");
		if (IsDead)
		{
			// GD.Print("I'm dead, ignoring");
			return;
		}

		if (typeOfBall == this.type)
		{
			GetStronger(ballPower);
		}
		else
		{
			TakeDamage(ballPower);
		}
	}

    private void GetStronger(int ballPower)
    {
		// GD.Print("Getting stronger!");

		if (CurrentStonksCooldown > 0.0f)
		{
			// GD.Print($"Still in cooldown for another {CurrentStonksCooldown}s, ignoring");
			return;
		}
		
		CurrentStonksCooldown = StonksCooldown;
		// GD.Print($"Resetting cooldown to {CurrentStonksCooldown}s!");
		
        Damage += (int)Math.Round(DamageScaling * ballPower);
		// GD.Print($"Damage is now equal to {Damage}!");

		var healthBonus = (int)Math.Round(HealthScaling * ballPower);
		MaxHealth += healthBonus;
		EmitSignal(SignalName.MaxHealthChanged, MaxHealth);
		// GD.Print($"MaxHealth is now equal to {MaxHealth}!");
		Health += healthBonus;
		// GD.Print($"Health is now equal to {Health}!");
		EmitSignal(SignalName.HealthChanged, Health);
		
		Scale *= SizeScaling;
    }
	
    private void TakeDamage(int ballPower)
    {
		// GD.Print("Taking damage!");

		if (CurrentDamageCooldown > 0.0f)
		{
			// GD.Print($"Still in cooldown for another {CurrentDamageCooldown}s, ignoring");
			return;
		}
		
		CurrentDamageCooldown = DamageCooldown;
		// GD.Print($"Resetting cooldown to {CurrentDamageCooldown}s!");

        Health -= ballPower;
		// GD.Print($"Health is now equal to {Health}!");
		if (IsDead)
		{
			Health = 0;
		}
		EmitSignal(SignalName.HealthChanged, Health);
		if (IsDead)
		{
			// GD.Print("I'm dead!");
			EmitSignal(SignalName.Died);
			QueueFree();
		}
    }


    public override void _Process(double delta)
	{
		CurrentStonksCooldown -= delta;
	}

	public override void _PhysicsProcess(double delta)
	{
		direction = (player.Position - this.Position).Normalized();
		KinematicCollision2D collision = MoveAndCollide(direction*Speed*(float)delta);
		
		if (collision != null)
		{
			if (collision.GetCollider().HasMethod("TakeDamage"))
			{
				collision.GetCollider().Call("TakeDamage", Damage);
			}
		}
	}
}
