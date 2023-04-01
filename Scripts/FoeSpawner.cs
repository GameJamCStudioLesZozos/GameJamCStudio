using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class FoeSpawner : Node2D
{
	[Export] public float baseTimer = 10.0f;
	[Export] public float spawnOffset = 0.0f;
	[Export] public Node2D player;
	[Export] public PackedScene Foe;

	private float timer;
	private RandomNumberGenerator rng = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = baseTimer;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer -= (float)delta;
		if (timer < 0.0f)
		{
			Spawn();
			timer = baseTimer;
		}
	}

	public bool Spawn()
	{
		PackedScene prefab = Foe;
		if (rng.Randf() > 0.5f)
		{
			prefab = Foe;
		}
		else
		{
			//prefab = ResourceLoader.Load<PackedScene>(Foes[1]);
		}
		Node2D monster = (Node2D)prefab.Instantiate();
		int rand = (int)GD.Randi() % 2;
		Node2D monsterToSpawn = (Node2D)monster.GetChild(rand);
		monsterToSpawn.Reparent(this);
		monster.QueueFree();
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
		monsterToSpawn.Transform = new Transform2D(0, newPos);
		//this.AddChild(monsterToSpawn);
		
		return true;
	}
}
