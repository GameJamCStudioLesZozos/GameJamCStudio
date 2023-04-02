using Godot;

public partial class IntroScript : Node2D
{
    [Export] public Control howToPlayParent;

    public void OnPlayButtonUp()
    {
        GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
    }

    public void OnShowHowToPlayButtonUp()
    {
        howToPlayParent.Visible = true;
    }

    public void OnHideHowToPlayButtonUp()
    {
        howToPlayParent.Visible = false;
    }

    public void OnQuitButtonUp()
    {
        GetTree().Quit();
    }
}
