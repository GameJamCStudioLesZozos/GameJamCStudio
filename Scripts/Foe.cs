using Godot;
using System;

public partial class Foe : AnimatableBody2D
{
	[Export] public float Speed = 500;
	[Export] public float StonksCooldown = 2.0f;
	public float ActualCooldown;
	
	[Export] public int Damage = 10;
	[Export] public int DamageBonus = 5;

	public Node2D player;
	private Vector2 direction;
	
	[Export] public Type type;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Node2D/Player");
		direction = (player.Position - this.Position).Normalized();
		ActualCooldown = StonksCooldown;
	}

	void Hit(Type typeofBall)
	{
		if (typeofBall == this.type)
		{
			if (ActualCooldown < 0.0f)
			{
				GD.Print("Stonks");
				ActualCooldown = StonksCooldown;
				Damage += DamageBonus;
			}
		}
		else
		{
			QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		direction = (player.Position - this.Position).Normalized();
		KinematicCollision2D collision = MoveAndCollide(direction*Speed*(float)delta);
		ActualCooldown -= (float)delta;
		if (collision != null)
		{
			if (collision.GetCollider().HasMethod("TakeDamage"))
			{
				collision.GetCollider().Call("TakeDamage", Damage);
			}
		}
	}
}
