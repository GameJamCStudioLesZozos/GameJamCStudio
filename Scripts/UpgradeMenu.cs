using Godot;
using System.Collections.Generic;

public partial class UpgradeMenu : Control
{
    [Export] private CanvasItem content;
    [Export] private Node itemsParent;
    [Export] private PackedScene upgradeMenuItem;
    [Export] private BallManager ballManager;

    public override void _Ready()
    {
        HideContent();
        ClearChildren();
    }

    public void OnPlayerLevelChanged(int level)
    {
        ShowContent();
        GenerateRandomUpgradeMenuItems();
    }

    public void OnUpgradeMenuItemClicked(BallUpgradeGD upgradeGD)
    {
        BallUpgrade upgrade = upgradeGD.Get();
        if (upgrade.Condition(ballManager))
        {
            upgrade.Effect(ballManager);
        }
        HideContent();
        GD.Print("Hide");
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
        var pickedUpgrades = new List<BallUpgrade>();
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
            item.Clicked += OnUpgradeMenuItemClicked;
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
