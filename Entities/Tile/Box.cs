namespace InfiniteSokoban.Entities.Tile;

public class Box : LevelEntity
{
    private Sprite _onGoal = null!;

    public bool OnGoal
    {
        get => _onGoal.Visible;
        set => _onGoal.Visible = value;
    }

    public override void _Ready()
    {
        _onGoal = GetNode<Sprite>("%OnGoal");
    }
}
