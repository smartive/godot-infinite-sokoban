using InfiniteSokoban.Globals;
using InfiniteSokoban.Globals.LevelGenerator;

namespace InfiniteSokoban.Entities.Level;

public class Level : Node2D
{
    private Node _floor = null!;
    private Node _walls = null!;
    private Node _goals = null!;
    private Node _boxes = null!;
    private Player _player = null!;
    private LevelGenerator _levelGenerator = null!;
    private GeneratedLevel? _currentLevel;

    private static readonly Dictionary<Cell, PackedScene> Objects = new()
    {
        { Cell.Floor, GD.Load<PackedScene>("res://Entities/Tile/Floor.tscn") },
        { Cell.Wall, GD.Load<PackedScene>("res://Entities/Tile/Wall.tscn") },
        { Cell.Goal, GD.Load<PackedScene>("res://Entities/Tile/Goal.tscn") },
        { Cell.Box, GD.Load<PackedScene>("res://Entities/Tile/Box.tscn") },
    };

    [Export(PropertyHint.Range, "1,4,1")]
    public int Width { get; set; } = 3;

    [Export(PropertyHint.Range, "1,4,1")]
    public int Height { get; set; } = 2;

    [Export(PropertyHint.Range, "1,3,1")]
    public int BoxCount { get; set; } = 2;

    private GeneratedLevel LoadedLevel => _currentLevel ?? throw new("Level not loaded.");

    public void Generate()
    {
        _currentLevel = _levelGenerator.GenerateLevel(Height, Width, BoxCount);

        foreach (var (x, y, cell) in LoadedLevel.IndexedIterator())
        {
            var cellPosition = new Vector2(x * 64, y * 64);

            var floor = Objects[Cell.Floor].Instance<Node2D>();
            floor.Position = cellPosition;
            _floor.AddChild(floor);

            switch (cell)
            {
                case Cell.Wall:
                    var wall = Objects[Cell.Wall].Instance<Node2D>();
                    wall.Position = cellPosition;
                    _walls.AddChild(wall);
                    break;
                case Cell.Goal:
                    {
                        var goal = Objects[Cell.Goal].Instance<Node2D>();
                        goal.Position = cellPosition;
                        _goals.AddChild(goal);
                    }
                    break;
                case Cell.Box:
                    {
                        var box = Objects[Cell.Box].Instance<Node2D>();
                        box.Position = cellPosition;
                        _boxes.AddChild(box);
                    }
                    break;
                case Cell.Player:
                    {
                        _player.Position = cellPosition;
                    }
                    break;
                case Cell.BoxOnGoal:
                    {
                        var box = Objects[Cell.Box].Instance<Node2D>();
                        box.Position = cellPosition;
                        _boxes.AddChild(box);

                        var goal = Objects[Cell.Goal].Instance<Node2D>();
                        goal.Position = cellPosition;
                        _goals.AddChild(goal);
                    }
                    break;

                case Cell.PlayerOnGoal:
                    {
                        var goal = Objects[Cell.Goal].Instance<Node2D>();
                        goal.Position = cellPosition;
                        _goals.AddChild(goal);
                        _player.Position = cellPosition;
                    }
                    break;
            }
        }
    }

    public override void _EnterTree()
    {
        _levelGenerator = GetNode<LevelGenerator>(AutoLoadIdentifier.LevelGenerator);
    }

    public override void _Ready()
    {
        _floor = GetNode<Node>("Floor");
        _walls = GetNode<Node>("Walls");
        _goals = GetNode<Node>("Goals");
        _boxes = GetNode<Node>("Boxes");
        _player = GetNode<Player>("Player");
    }
}
