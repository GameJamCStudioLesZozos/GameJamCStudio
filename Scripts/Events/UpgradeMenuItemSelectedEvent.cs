using Events;

public class UpgradeMenuItemSelectedEvent : Event
{
    public UpgradeMenuItemSelectedEvent() { throw new System.InvalidOperationException(); }
    public UpgradeMenuItemSelectedEvent(BallUpgrade upgrade)
    {
        Upgrade = upgrade;
    }

    public BallUpgrade Upgrade { get; }
}
