using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class FoeSpawner : Node2D
{
	[Export] public double baseTimer = 10.0;
	[Export] public float spawnOffset = 0.0f;
	[Export] public Node2D player;
	[Export] public PackedScene IceFoe;
	[Export] public PackedScene FireFoe;

	private double timer;
	private RandomNumberGenerator rng = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = baseTimer;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer -= delta;
		if (timer < 0.0)
		{
			Spawn();
			timer = baseTimer;
		}
	}

	public bool Spawn()
	{
		PackedScene prefab = null;
		if (rng.Randf() > 0.5f)
		{
			prefab = IceFoe;
		}
		else
		{
			prefab = FireFoe;
		}
		Node2D monster = (Node2D)prefab.Instantiate();
		GetParent().AddChild(monster);

		var viewportSize = GetViewport().GetVisibleRect().Size;
		var newPos = player.Position;
		if (rng.Randf() > 0.5f)
		{
			// Top or bottom
			newPos.X += (rng.Randf() - 0.5f) * viewportSize.X;
			newPos.Y += (rng.Randf() > 0.5f ? +1.0f : -1.0f) * (0.5f * viewportSize.Y + spawnOffset);
		}
		else
		{
			// Left or right
			newPos.X += (rng.Randf() > 0.5f ? +1.0f : -1.0f) * (0.5f * viewportSize.X + spawnOffset);
			newPos.Y += (rng.Randf() - 0.5f) * viewportSize.Y;
		}
		monster.Transform = new Transform2D(0, newPos);
		
		return true;
	}
}
