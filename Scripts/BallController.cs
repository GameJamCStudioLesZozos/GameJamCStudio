using Godot;
using System;

public partial class BallController : RigidBody2D
{
	[Export] public double angle = 0;
	[Export] public double rotationSpeed = 1.5;
	[Export] public float radius = 100;
	[Export] public Type type;
	[Export] public int damage = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var oldPos = this.Position;
		angle += rotationSpeed * delta;
		var newPos = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * radius;
		
		var collision = MoveAndCollide(newPos - oldPos, true);
		Position = newPos;
		if (collision != null)
		{
			if (collision.GetCollider().HasMethod("Hit"))
			{
				collision.GetCollider().Call("Hit", Variant.From(type), damage);
			}
		}
	}

    public void OnPlayerDied()
    {
        QueueFree();
    }
}
