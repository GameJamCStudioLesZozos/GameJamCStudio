using Godot;
using System;
using System.Collections.Generic;

public partial class PowerUpManager : Node2D
{
	[Export] private BallManager ballManager;

	private class BallUpgrade
	{
		public string Description;
		public Action<BallManager> Effect;
		public Func<BallManager, bool> Condition;
	}

	private BallUpgrade[] upgrades = {
		new BallUpgrade {
			Description = "Increase radius",
			Effect = (BallManager manager) => manager.IncreaseRadius(),
			Condition = (BallManager manager) => !manager.IsRadiusMax()
		},
		new BallUpgrade {
			Description = "Decrease radius",
			Effect = (BallManager manager) => manager.DecreaseRadius(),
			Condition = (BallManager manager) => !manager.IsRadiusMin()
		},
		new BallUpgrade {
			Description = "Increase rotation speed",
			Effect = (BallManager manager) => manager.IncreaseRotationSpeed(),
			Condition = (BallManager manager) => !manager.IsRotationSpeedMax()
		},
		new BallUpgrade {
			Description = "Decrease rotation speed",
			Effect = (BallManager manager) => manager.DecreaseRotationSpeed(),
			Condition = (BallManager manager) => !manager.IsRotationSpeedMin()
		},
		new BallUpgrade {
			Description = "Add 2 more balls",
			Effect = (BallManager manager) => manager.AddTwoMoreBalls(),
			Condition = (BallManager manager) => !manager.IsNumberOfBallsMax()
		},
		new BallUpgrade {
			Description = "Remove 2 balls",
			Effect = (BallManager manager) => manager.RemoveTwoBalls(),
			Condition = (BallManager manager) => !manager.IsNumberOfBallsMin()
		},
		new BallUpgrade {
			Description = "Increase balls power",
			Effect = (BallManager manager) => manager.IncreaseBallsPower(),
			Condition = (BallManager manager) => true
		}
	};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayerLevelChanged(int newLevel)
	{
		while (true)
		{
			var upgrade = upgrades[GD.Randi() % upgrades.Length];
			if (!upgrade.Condition(ballManager))
				continue;

			upgrade.Effect(ballManager);
			break;
		}
	}
}
