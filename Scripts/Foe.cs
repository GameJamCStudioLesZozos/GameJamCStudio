using Godot;
using System;

public partial class Foe : AnimatableBody2D
{
	[Export] public float Speed = 500;
	
	public Node2D player;
	private Vector2 direction;
	
	[Export] public Type type;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Node2D/Player");
		direction = (player.Position - this.Position).Normalized();
	}

	void Hit(Vector2 position, float direction)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		direction = (player.Position - this.Position).Normalized();
		MoveAndCollide(direction*Speed*(float)delta);
	}
}
