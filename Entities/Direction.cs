namespace InfiniteSokoban.Entities;

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public static class DirectionExtensions
{
    public static (int X, int Y) ToCoords(this Direction direction) =>
        direction switch
        {
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public static (int X, int Y) Move(this Direction direction, (int X, int Y) coords)
    {
        var (x, y) = direction.ToCoords();
        return (coords.X + x, coords.Y + y);
    }
    
    /// <summary>
    /// Move the coordinates into the given direction. However, if the new coordinates are outside the given bounds,
    /// null is returned. To calculate the boundaries, 0 based indices are used.
    /// </summary>
    public static (int X, int Y)? Move(this Direction direction, (int X, int Y) coords, int width, int height)
    {
        var (x, y) = direction.ToCoords();
        var (newX, newY) = (coords.X + x, coords.Y + y);
        if (newX < 0 || newX >= width - 1 || newY < 0 || newY >= height - 1)
        {
            return null;
        }

        return (newX, newY);
    }
}
