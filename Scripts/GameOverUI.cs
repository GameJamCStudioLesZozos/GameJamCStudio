using Godot;

public partial class GameOverUI : Control
{
    [Export]
    private CanvasItem content;

    public override void _Ready()
    {
        content.Visible = false;
    }

    public void OnPlayerDied()
    {
        content.Visible = true;
    }

    public void OnRestartButtonUp()
    {
        GetTree().ReloadCurrentScene();
    }

    public void OnQuitButtonUp()
    {
        GetTree().Quit();
    }
}
