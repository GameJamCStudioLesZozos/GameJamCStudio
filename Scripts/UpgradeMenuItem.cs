using Godot;
using Events;

public partial class UpgradeMenuItem : Control
{
    [Export] private CanvasItem panelHover;
    [Export] private RichTextLabel title;
    [Export] private TextureRect image;

    private bool _isMouseOver;
    private BallUpgrade _upgrade;

	public override void _Process(double delta)
	{
        if (_isMouseOver && Input.IsActionJustPressed("Click"))
        {
            this.Publish(new UpgradeMenuItemSelectedEvent(_upgrade));
        }
	}

    public void SetUpgrade(BallUpgrade upgrade)
    {
        _upgrade = upgrade;
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
