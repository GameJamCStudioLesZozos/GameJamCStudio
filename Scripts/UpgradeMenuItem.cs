using Godot;
public partial class UpgradeMenuItem : Control
{
    [Export] private CanvasItem panelHover;
    [Export] private RichTextLabel title;
    [Export] private TextureRect image;

    [Signal] public delegate void ClickedEventHandler(BallUpgradeGD upgradeGD);

    private bool _isMouseOver;
    private BallUpgradeGD _upgrade;

	public override void _Process(double delta)
	{
        if (_isMouseOver && Input.IsActionJustPressed("Click"))
        {
            EmitSignal(SignalName.Clicked, _upgrade);
            GD.Print($"Clicked on {_upgrade.Get().Description}");
        }
	}

    public void SetUpgrade(BallUpgrade upgrade)
    {
        _upgrade = new BallUpgradeGD(upgrade);
        title.Text = upgrade.Description;
        image.Texture = upgrade.Image;
    }

    public void OnMouseEntered()
    {
        _isMouseOver = true;
        panelHover.Visible = true;
    }

    public void OnMouseExited()
    {
        _isMouseOver = false;
        panelHover.Visible = false;
    }
}
