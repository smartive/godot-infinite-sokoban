using InfiniteSokoban.Data;
using InfiniteSokoban.Entities.Tile;
using InfiniteSokoban.Extensions;
using InfiniteSokoban.LevelGenerator;

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

    private List<Coordinates> _levelGoals = [];
    private GeneratedLevel? _generatedLevel;
    private CoordinateArray<Cell?>? _blockingEntities;
    private int _boxesOnGoal;

    [Signal]
    public delegate void LevelFinished();

    [Signal]
    public delegate void LevelProgress(int onGoal, int totalGoals);

    private static readonly Dictionary<Cell, PackedScene> Objects = new()
    {
        { Cell.Floor, GD.Load<PackedScene>("res://Entities/Tile/Floor.tscn") },
        { Cell.Wall, GD.Load<PackedScene>("res://Entities/Tile/Wall.tscn") },
        { Cell.Goal, GD.Load<PackedScene>("res://Entities/Tile/Goal.tscn") },
        { Cell.Box, GD.Load<PackedScene>("res://Entities/Tile/Box.tscn") },
    };

    [Export(PropertyHint.Range, "1,4,1")]
    public int XRooms { get; set; } = 3;

    [Export(PropertyHint.Range, "1,4,1")]
    public int YRooms { get; set; } = 2;

    [Export(PropertyHint.Range, "1,3,1")]
    public int BoxCount { get; set; } = 2;

    public Vector2 Size => new((XRooms * 3 + 2) * 64, (YRooms * 3 + 2) * 64);

    private CoordinateArray<Cell?> BlockingEntities => _blockingEntities ?? new CoordinateArray<Cell?>(XRooms, YRooms);
    private GeneratedLevel LoadedLevel => _generatedLevel ?? throw new("Level is null.");

    public void Generate()
    {
        _generatedLevel = LevelGenerator.LevelGenerator.GenerateLevel(XRooms, YRooms, BoxCount);
        _blockingEntities = new CoordinateArray<Cell?>(LoadedLevel.Width, LoadedLevel.Height);
        _levelGoals = _generatedLevel.IndexedIterator()
            .Where(cell => cell.Data.IsGoal())
            .Select(cell => (cell.X, cell.Y)).ToList();
    }

    public void Start() => Reset();

    public void Reset()
    {
        _floor.FreeAllChildren();
        _walls.FreeAllChildren();
        _goals.FreeAllChildren();
        _boxes.FreeAllChildren();
        _player.Look(Direction.Down);
        BlockingEntities.Clear();
        _boxesOnGoal = 0;
        DrawLevel();
        EmitSignal(nameof(LevelProgress), _boxesOnGoal, _levelGoals.Count);
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

        if (_objectMover.IsActive() || @event.InputPlayerDirection(true) is not { } direction)
        {
            // Don't allow input while the player / boxes are moving.
            return;
        }

        var (x, y) = _player.GridPosition;

        if (direction.Move((x, y), LoadedLevel.Width, LoadedLevel.Height) is not { } newPos)
        {
            // Don't allow the player to move outside the level.
            _player.Look(direction);
            return;
        }

        if (BlockingEntities[newPos] == Cell.Wall)
        {
            // The player cannot move through walls.
            _player.Look(direction);
            return;
        }

        if (BlockingEntities[newPos]?.CouldBlockPlayer() == true &&
            (direction.Move(newPos, LoadedLevel.Width, LoadedLevel.Height) is null ||
             (direction.Move(newPos, LoadedLevel.Width, LoadedLevel.Height) is { } overNextPos &&
              BlockingEntities[overNextPos]?.CouldBlockPlayer() == true)))
        {
            // The player cannot push boxes into walls or other boxes.
            _player.Look(direction);
            return;
        }

        // Move the player
        _player.Move(direction);
        _objectMover.InterpolateProperty(
            _player,
            "position",
            _player.Position,
            CoordsToPos(newPos),
            moveDuration);

        // Move the box if there is one.
        if (BlockingEntities[newPos] == Cell.Box)
        {
            var box = _boxes.GetChildren().OfType<LevelEntity>().First(l => l.GridPosition == newPos);
            var newBoxPos = direction.Move(box.GridPosition);
            BlockingEntities.Swap(box.GridPosition, newBoxPos);
            _objectMover.InterpolateProperty(
                box,
                "position",
                box.Position,
                CoordsToPos(newBoxPos),
                moveDuration);
        }

        _objectMover.Start();
    }

    private void OnObjectMoved(Object obj, NodePath _)
    {
        switch (obj)
        {
            case Player.Player p:
                p.Stop();
                break;
            case Box b:
                var isNowOnGoal = _levelGoals.Contains(b.GridPosition);
                switch (isNowOnGoal)
                {
                    case true when !b.OnGoal:
                        _boxesOnGoal += 1;
                        EmitSignal(nameof(LevelProgress), _boxesOnGoal, _levelGoals.Count);
                        break;
                    case false when b.OnGoal:
                        _boxesOnGoal -= 1;
                        EmitSignal(nameof(LevelProgress), _boxesOnGoal, _levelGoals.Count);
                        break;
                }

                b.OnGoal = isNowOnGoal;

                if (_boxesOnGoal == _levelGoals.Count)
                {
                    EmitSignal(nameof(LevelFinished));
                }

                break;
        }
    }

    private void DrawLevel()
    {
        foreach (var (x, y, cell) in LoadedLevel.IndexedIterator())
        {
            var floor = Objects[Cell.Floor].Instance<LevelEntity>();
            floor.GridPosition = (x, y);
            _floor.AddChild(floor);

            switch (cell)
            {
                case Cell.Wall:
                    {
                        var wall = Objects[Cell.Wall].Instance<LevelEntity>();
                        wall.GridPosition = (x, y);
                        _walls.AddChild(wall);
                        BlockingEntities[x, y] = Cell.Wall;
                    }
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
                        var box = Objects[Cell.Box].Instance<Box>();
                        box.GridPosition = (x, y);
                        _boxes.AddChild(box);
                        BlockingEntities[x, y] = Cell.Box;
                    }
                    break;
                case Cell.Player:
                    {
                        _player.GridPosition = (x, y);
                    }
                    break;
                case Cell.BoxOnGoal:
                    {
                        var box = Objects[Cell.Box].Instance<Box>();
                        box.GridPosition = (x, y);
                        box.InitialOnGoal = true;
                        _boxes.AddChild(box);
                        BlockingEntities[x, y] = Cell.Box;
                        _boxesOnGoal += 1;
                        EmitSignal(nameof(LevelProgress), _boxesOnGoal, _levelGoals.Count);

                        var goal = Objects[Cell.Goal].Instance<LevelEntity>();
                        goal.GridPosition = (x, y);
                        _goals.AddChild(goal);
                    }
                    break;

                case Cell.PlayerOnGoal:
                    {
                        var goal = Objects[Cell.Goal].Instance<LevelEntity>();
                        goal.GridPosition = (x, y);
                        _player.GridPosition = (x, y);
                    }
                    break;
            }
        }
    }

    private static Vector2 CoordsToPos(Coordinates gridPosition) =>
        new(gridPosition.X * 64, gridPosition.Y * 64);
}
