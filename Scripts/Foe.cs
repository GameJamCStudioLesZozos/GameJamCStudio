using Godot;
using System;

public partial class Foe : RigidBody2D
{
	public int Speed = 5;
	
	[Export]
	public Type type;
	public void Start(Vector2 position, float direction)
	{
		Position = position;
		LinearVelocity = new Vector2(Speed, 0).Rotated(direction);
	}
	public override void _PhysicsProcess(double delta)
	{
		//var collision = MoveAndCollide(LinearVelocity);
		
	}
}
