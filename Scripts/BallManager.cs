using Godot;
using System;
using System.Collections.Generic;

public partial class BallManager : Node2D
{
	[Export] public PackedScene IceBall;
	[Export] public PackedScene FireBall;

	private double angle = 0;
	[Export] public double rotationSpeed = 1.5;
	[Export] public double minRotationSpeed = 0.5;
	[Export] public double maxRotationSpeed = 5.0;
	[Export] public double rotationSpeedChange = 0.5;
	[Export] private float radius = 100;
	[Export] private float minRadius = 50;
	[Export] private float maxRadius = 300;
	[Export] private float radiusChange = 50;
	[Export] private int ballDamage = 10;
	[Export] private int ballDamageChange = 5;
	[Export] private int maxBallCount = 10;
	
	private List<BallController> balls = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Singletons.Register(this);
		AddBall(IceBall);
		AddBall(FireBall);
	}

    private void AddBall(PackedScene ballPrefab)
    {
		var newBall = (BallController)ballPrefab.Instantiate();
		newBall.radius = radius;
		newBall.damage = ballDamage;
        AddChild(newBall);
		balls.Add(newBall);
		UpdateBallsAngle();
    }

    private void UpdateBallsAngle()
    {
		float angleDelta = 2.0f * (float)Math.PI / (float)balls.Count;
        for (int i = 0; i < balls.Count; ++i)
		{
			balls[i].angle = angle + (float)i * angleDelta;
		}
    }
	
    private void UpdateBallsRadius()
    {
        foreach (var ball in balls)
		{
			ball.radius = radius;
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
		balls.Clear();
    }

    internal void IncreaseRadius()
    {
        radius += radiusChange;
		UpdateBallsRadius();
    }

    internal void DecreaseRadius()
    {
        radius -= radiusChange;
		UpdateBallsRadius();
    }

    internal void IncreaseRotationSpeed()
    {
        rotationSpeed += rotationSpeedChange;
    }

    internal void DecreaseRotationSpeed()
    {
        rotationSpeed -= rotationSpeedChange;
    }

    internal void IncreaseBallsPower()
    {
        foreach (var ball in balls)
		{
			ball.damage += ballDamageChange;
		}
    }

    internal void AddTwoMoreBalls()
    {
        int newBallCount = balls.Count + 2;
		UnspawnAllBalls();
        for (int i = 0; i < newBallCount / 2; i++)
        {
            AddBall(IceBall);
        }
        for (int i = newBallCount / 2; i < newBallCount; i++)
        {
            AddBall(FireBall);
        }
    }

    internal bool IsRadiusMax()
    {
        return radius >= maxRadius;
    }

    internal bool IsRadiusMin()
    {
        return radius <= minRadius;
    }

    internal bool IsRotationSpeedMax()
    {
        return rotationSpeed >= maxRotationSpeed;
    }

    internal bool IsRotationSpeedMin()
    {
        return rotationSpeed <= minRotationSpeed;
    }

    internal bool IsNumberOfBallsMax()
    {
        return balls.Count >= maxBallCount;
    }

    internal void RemoveTwoBalls()
    {
        int prevBallCount = balls.Count;
		UnspawnAllBalls();
		for (int i = 0; i < prevBallCount - 2; ++i)
		{
			if (i % 2 == 0)
			{
				AddBall(IceBall);
			}
			else
			{
				AddBall(FireBall);
			}
		}
    }

    internal bool IsNumberOfBallsMin()
    {
        return balls.Count <= 2;
    }
}
