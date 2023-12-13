using InfiniteSokoban.Entities.Level;

namespace InfiniteSokoban.Scenes.Game;

public class Game : Node
{
    private Level _level = null!;
    private Label _levelInfo = null!;
    private Label _goalInfo = null!;

    public override void _Ready()
    {
        _levelInfo = GetNode<Label>("%LevelInfoValue");
        _goalInfo = GetNode<Label>("%GoalInfoValue");

        _level = GetNode<Level>("%Level");

        // TODO: generate level based on settings.
        _level.Generate();
        _goalInfo.Text = "0/0 Goals";
        _levelInfo.Text = $"Width: {_level.XRooms} Height: {_level.YRooms} Boxes: {_level.BoxCount}";

        _level.Position = GetViewport().Size / 2 - _level.Size / 2;

        _level.Start();
    }
    
    private void OnResetPressed()
    {
        _level.Reset();
    }
    
    private void OnBackPressed()
    {
        OnLevelFinished();
    }

    private void OnLevelProgress(int onGoal, int totalGoals)
    {
        _goalInfo.Text = $"{onGoal}/{totalGoals} Goals";
    }

    private void OnLevelFinished()
    {
        // TODO: make nice ending screen and retry stuff.
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
