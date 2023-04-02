using Godot;
using System;

public partial class BallController : Node2D
{
	[Export] public double angle = 0;
	[Export] public float radius = 100;
	[Export] public Type type;
	[Export] public int damage = 10;
	[Export] private Area2D hitbox;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		CheckHitBoxCollisions();
    }

    public void OnPlayerDied()
    {
        QueueFree();
    }

	public void CheckHitBoxCollisions()
	{
		var hurtboxes = hitbox.GetOverlappingAreas();
		foreach (var hurtbox in hurtboxes)
		{
			var parent = hurtbox.GetParent();
			if (parent != null && parent.HasMethod("Hit"))
			{
				parent.Call("Hit", Variant.From(type), damage);
			}
		}
	}
}
