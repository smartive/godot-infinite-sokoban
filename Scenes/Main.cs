namespace InfiniteSokoban.Scenes;

public class Main : Control
{
    private void StartGamePressed()
    {
        GetTree().ChangeScene("res://Scenes/Game/Game.tscn");
    }
}
