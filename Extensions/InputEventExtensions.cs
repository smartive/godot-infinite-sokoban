using InfiniteSokoban.Entities;

namespace InfiniteSokoban.Extensions;

public static class InputEventExtensions
{
    public static Direction? PlayerMoveDirection(this InputEvent ev) => true switch
    {
        true when ev.IsActionPressed(GameInputMap.PlayerUp) => Direction.Up,
        true when ev.IsActionPressed(GameInputMap.PlayerDown) => Direction.Down,
        true when ev.IsActionPressed(GameInputMap.PlayerLeft) => Direction.Left,
        true when ev.IsActionPressed(GameInputMap.PlayerRight) => Direction.Right,
        _ => null,
    };
}
