using Godot;
using System;

public partial class BallController : RigidBody2D
{
	[Export] public double angle = 0;
	[Export] public double rotationSpeed = 1.5;
	[Export] public float radius = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		angle += rotationSpeed * delta;
		this.Position = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * radius;
	}
}
