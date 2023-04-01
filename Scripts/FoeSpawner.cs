using Godot;
using System;

public partial class FoeSpawner : Node
{
	[Export]
	public float baseTimer = 10.0f;
	public float timer;
	[Export]
	public float spawnOffset = 50.0f;
	
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
			this.Spawn();
			timer = baseTimer;
		}
	}

	public bool Spawn()
	{
		Texture2D tex = (Texture2D)GD.Load("res://icon.svg");
		Sprite2D monster = new Sprite2D();
		monster.Texture = tex;
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
