using InfiniteSokoban.Extensions;
using InfiniteSokoban.Globals;
using InfiniteSokoban.Globals.LevelGenerator;

using Object = Godot.Object;

namespace InfiniteSokoban.Entities.Level;

public class Level : Node2D
{
    private Node _floor = null!;
    private Node _walls = null!;
    private Node _goals = null!;
    private Node _boxes = null!;
    private Player.Player _player = null!;
    private Tween _objectMover = null!;

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
            var floor = Objects[Cell.Floor].Instance<LevelEntity>();
            floor.GridPosition = (x, y);
            _floor.AddChild(floor);

            switch (cell)
            {
                case Cell.Wall:
                    var wall = Objects[Cell.Wall].Instance<LevelEntity>();
                    wall.GridPosition = (x, y);
                    _walls.AddChild(wall);
                    break;
                case Cell.Goal:
                    {
                        var goal = Objects[Cell.Goal].Instance<LevelEntity>();
                        goal.GridPosition = (x, y);
                        _goals.AddChild(goal);
                    }
                    break;
                case Cell.Box:
                    {
                        var box = Objects[Cell.Box].Instance<LevelEntity>();
                        box.GridPosition = (x, y);
                        _boxes.AddChild(box);
                    }
                    break;
                case Cell.Player:
                    {
                        _player.GridPosition = (x, y);
                    }
                    break;
                case Cell.BoxOnGoal:
                    {
                        var box = Objects[Cell.Box].Instance<LevelEntity>();
                        box.GridPosition = (x, y);
                        _boxes.AddChild(box);

                        var goal = Objects[Cell.Goal].Instance<LevelEntity>();
                        goal.GridPosition = (x, y);
                        _goals.AddChild(goal);
                    }
                    break;

                case Cell.PlayerOnGoal:
                    {
                        var goal = Objects[Cell.Goal].Instance<LevelEntity>();
                        goal.GridPosition = (x, y);
                        _goals.AddChild(goal);
                        _player.GridPosition = (x, y);
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
        _floor = GetNode<Node>("%Floor");
        _walls = GetNode<Node>("%Walls");
        _goals = GetNode<Node>("%Goals");
        _boxes = GetNode<Node>("%Boxes");
        _player = GetNode<Player.Player>("%Player");
        _objectMover = GetNode<Tween>("%ObjectMover");
    }

    public override void _Input(InputEvent @event)
    {
        const float moveDuration = .4f;

        if (_objectMover.IsActive() || @event.PlayerMoveDirection() is not { } direction)
        {
            // Don't allow input while the player / boxes are moving.
            return;
        }

        var (x, y) = _player.GridPosition;
        
        if (direction.Move((x,y), LoadedLevel.Width, LoadedLevel.Height) is not { } aa)
        {
            // Don't allow the player to move outside the level.
            return;
        }
        
        var (nexX, newY) = aa;

        switch (true)
        {
            case true when @event.IsActionPressed(GameInputMap.PlayerUp):
                {
                    _player.Move(Direction.Up);
                    _objectMover.InterpolateProperty(
                        _player,
                        "position",
                        _player.Position,
                        CoordsToPos((x, y - 1)),
                        moveDuration);
                }
                break;
            case true when @event.IsActionPressed(GameInputMap.PlayerDown):
                {
                    _player.Move(Direction.Down);
                    _objectMover.InterpolateProperty(
                        _player,
                        "position",
                        _player.Position,
                        CoordsToPos((x, y + 1)),
                        moveDuration);
                }
                break;
            case true when @event.IsActionPressed(GameInputMap.PlayerLeft):
                {
                    _player.Move(Direction.Left);
                    _objectMover.InterpolateProperty(
                        _player,
                        "position",
                        _player.Position,
                        CoordsToPos((x - 1, y)),
                        moveDuration);
                }
                break;
            case true when @event.IsActionPressed(GameInputMap.PlayerRight):
                {
                    _player.Move(Direction.Right);
                    _objectMover.InterpolateProperty(
                        _player,
                        "position",
                        _player.Position,
                        CoordsToPos((x + 1, y)),
                        moveDuration);
                }
                break;
        }

        _objectMover.Start();
    }
    
    private void MoveObjects(){}

    private void OnObjectMoved(Object obj, NodePath _)
    {
        if (obj is Player.Player p)
        {
            p.Stop();
        }
    }

    private static Vector2 CoordsToPos((int X, int Y) gridPosition) =>
        new(gridPosition.X * 64, gridPosition.Y * 64);
}
