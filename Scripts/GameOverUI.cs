using Godot;

public partial class GameOverUI : Control
{
    [Export]
    private CanvasItem content;

    [Export]
    private PlayerController player;

    [Export]
    private RichTextLabel scoreText;

    public override void _Ready()
    {
        content.Visible = false;
    }

    public void OnPlayerDied()
    {
        content.Visible = true;
        scoreText.Text = $"Score: {player.GetScore()}";
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
