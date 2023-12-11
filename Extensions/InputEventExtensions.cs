using InfiniteSokoban.Entities;

namespace InfiniteSokoban.Extensions;

public static class InputEventExtensions
{
    public static Direction? InputPlayerDirection(this InputEvent ev, bool allowEcho = false) => true switch
    {
        true when ev.IsActionPressed(GameInputMap.PlayerUp, allowEcho) => Direction.Up,
        true when ev.IsActionPressed(GameInputMap.PlayerDown, allowEcho) => Direction.Down,
        true when ev.IsActionPressed(GameInputMap.PlayerLeft, allowEcho) => Direction.Left,
        true when ev.IsActionPressed(GameInputMap.PlayerRight, allowEcho) => Direction.Right,
        _ => null,
    };
}
