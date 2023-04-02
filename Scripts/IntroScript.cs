using Godot;

public partial class IntroScript : Node2D
{
    public void OnPlayButtonUp()
    {
        GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
    }

    public void OnQuitButtonUp()
    {
        GetTree().Quit();
    }
}
