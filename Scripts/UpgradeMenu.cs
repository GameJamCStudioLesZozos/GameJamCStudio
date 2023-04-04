using Events;
using Godot;
using System.Collections.Generic;

public partial class UpgradeMenu : Control,
    IEventHandler<UpgradeMenuItemSelectedEvent>
{
    [Export] private CanvasItem content;
    [Export] private Node itemsParent;
    [Export] private PackedScene upgradeMenuItem;

    public override void _Ready()
    {
        this.Subscribe<UpgradeMenuItemSelectedEvent>();
        HideContent();
        ClearChildren();
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
        const int nbUpgrades = 3;
        HashSet<BallUpgrade> pickedUpgrades = new();
        for (int i = 0; i < nbUpgrades; i++)
        {
            var item = upgradeMenuItem.Instantiate<UpgradeMenuItem>();
            itemsParent.AddChild(item);
            BallUpgrade upgrade = null;
            do
            {
                upgrade = BallUpgrades.PickRandomUpgrade();
            }
            while (pickedUpgrades.Contains(upgrade));
            item.SetUpgrade(upgrade);
        }
    }

    private void ClearChildren()
    {
        foreach (Node child in itemsParent.GetChildren())
        {
            child.QueueFree();
        }
    }
}
