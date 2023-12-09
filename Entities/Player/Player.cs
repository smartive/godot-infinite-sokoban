using InfiniteSokoban;

public class Player : Node2D
{
    private AnimatedSprite _animation = null!;

    private const string StandLeft = "stand_left";

    private const string StandRight = "stand_right";

    private const string StandUp = "stand_up";

    private const string StandDown = "stand_down";

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite>("%Animation");
    }

    public override void _Input(InputEvent @event)
    {
        switch (true)
        {
            case true when @event.IsActionPressed(GameInputMap.PlayerUp):
                _animation.Animation = StandUp;
                break;
            case true when @event.IsActionPressed(GameInputMap.PlayerDown):
                _animation.Animation = StandDown;
                break;
            case true when @event.IsActionPressed(GameInputMap.PlayerLeft):
                _animation.Animation = StandLeft;
                break;
            case true when @event.IsActionPressed(GameInputMap.PlayerRight):
                _animation.Animation = StandRight;
                break;
        }
    }
}
