using Godot;
using System;

public partial class FoeSpawner : Node
{
	[Export(PropertyHint.LocalizableString,"Timer of Spawn")]
	public float baseTimer = 10.0f;
	public float timer;
	[Export]
	public float spawnOffset = 0.0f;

	[Export]
	public float[] weights;
	
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
		PackedScene prefab = ResourceLoader.Load<PackedScene>("res://Prefabs/Foe.tscn");
		Node2D monster = (Node2D)prefab.Instantiate();
		var rng = new RandomNumberGenerator();
		float width = 0;
		float height = 0;
		if (rng.Randf() > 0.5f)
		{
			
			width = rng.Randf() * GetViewport().GetVisibleRect().Size.X + spawnOffset;
			height = rng.Randf() > 0.5f ?-spawnOffset:GetViewport().GetVisibleRect().Size.Y + spawnOffset;
		}
		else
		{
			height = rng.Randf() * GetViewport().GetVisibleRect().Size.Y + spawnOffset;
			width = rng.Randf() > 0.5f ?-spawnOffset:GetViewport().GetVisibleRect().Size.X + spawnOffset;
		}
		monster.Transform = new Transform2D(0, new Vector2(width, height));
		AddChild(monster);
		return true;
	}
}
