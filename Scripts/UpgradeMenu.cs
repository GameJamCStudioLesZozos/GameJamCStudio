using Events;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class UpgradeMenu : Control,
    IEventHandler<UpgradeMenuItemSelectedEvent>
{
    [Export] private CanvasItem content;
    [Export] private Node itemsParent;

    public override void _Ready()
    {
        this.Subscribe<UpgradeMenuItemSelectedEvent>();
        HideContent();
    }

    public void OnPlayerLevelChanged(int level)
    {
        ShowContent();
        GenerateRandomUpgradeMenuItems();
    }

    void IEventHandler<UpgradeMenuItemSelectedEvent>.Handle(UpgradeMenuItemSelectedEvent @event)
    {
        var ballManager = Singletons.Get<BallManager>();
        if (@event.Upgrade.Condition(ballManager))
        {
            @event.Upgrade.Effect(ballManager);
        }
        HideContent();
    }

    private void ShowContent()
    {
        content.Visible = true;
        GetTree().Paused = true;
    }

    private void HideContent()
    {
        content.Visible = false;
        GetTree().Paused = false;
    }

    private void GenerateRandomUpgradeMenuItems()
    {
        UpgradeMenuItem[] menuItems = itemsParent.GetChildren().OfType<UpgradeMenuItem>().ToArray();
        HashSet<BallUpgrade> pickedUpgrades = new();
        foreach (UpgradeMenuItem menuItem in menuItems)
        {
            BallUpgrade upgrade = null;
            do
            {
                upgrade = BallUpgrades.PickRandomUpgrade();
            }
            while (pickedUpgrades.Contains(upgrade));
            menuItem.SetUpgrade(upgrade);
        }
    }
}
