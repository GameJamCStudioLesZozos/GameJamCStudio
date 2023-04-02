using Godot;
using System;
using System.Collections.Generic;

public partial class BallManager : Node2D
{
	[Export] public PackedScene IceBall;
	[Export] public PackedScene FireBall;

	private double angle = 0;
	[Export] public double rotationSpeed = 1.5;
	[Export] private float radius = 100;
	
	private List<BallController> balls = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddBall(IceBall);
		AddBall(FireBall);
	}

    private void AddBall(PackedScene ballPrefab)
    {
		var newBall = (BallController)ballPrefab.Instantiate();
		newBall.radius = radius;
        AddChild(newBall);
		balls.Add(newBall);
		UpdateBallsAngle();
    }

    private void UpdateBallsAngle()
    {
		float angleDelta = 2.0f * (float)Math.PI / balls.Count;
        for (int i = 0; i < balls.Count; ++i)
		{
			balls[i].angle = angle + i * angleDelta;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		angle += rotationSpeed * delta;
		angle %= 2.0f * Math.PI;
		UpdateBallsAngle();
	}

	public void OnPlayerDied()
	{
		UnspawnAllBalls();
	}

    private void UnspawnAllBalls()
    {
        foreach (var ball in balls)
		{
			ball.QueueFree();
		}
    }
}
