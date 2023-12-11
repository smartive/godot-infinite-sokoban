namespace InfiniteSokoban.Entities.Player;

public class Player : LevelEntity
{
    private AnimatedSprite _animation = null!;
    private Direction _direction;

    private const string StandLeft = "stand_left";

    private const string StandRight = "stand_right";

    private const string StandUp = "stand_up";

    private const string StandDown = "stand_down";

    private const string MoveLeft = "move_left";

    private const string MoveRight = "move_right";

    private const string MoveUp = "move_up";

    private const string MoveDown = "move_down";

    public void Look(Direction direction)
    {
        _direction = direction;
        Stop();
    }
    
    public void Move(Direction direction)
    {
        _direction = direction;
        switch (_direction)
        {
            case Direction.Left:
                _animation.Play(MoveLeft);
                break;
            case Direction.Right:
                _animation.Play(MoveRight);
                break;
            case Direction.Up:
                _animation.Play(MoveUp);
                break;
            case Direction.Down:
                _animation.Play(MoveDown);
                break;
        }
    }

    public void Stop()
    {
        switch (_direction)
        {
            case Direction.Left:
                _animation.Play(StandLeft);
                break;
            case Direction.Right:
                _animation.Play(StandRight);
                break;
            case Direction.Up:
                _animation.Play(StandUp);
                break;
            case Direction.Down:
                _animation.Play(StandDown);
                break;
        }
    }

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite>("%Animation");
    }
}
