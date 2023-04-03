using Godot;

public partial class PowerUpManager : Node2D
{
	[Export] private BallManager ballManager;

	public void OnPlayerLevelChanged(int newLevel)
	{
		while (true)
		{
			var upgrade = BallUpgrades.PickRandomUpgrade();
			if (!upgrade.Condition(ballManager))
				continue;

			upgrade.Effect(ballManager);
			break;
		}
	}
}
