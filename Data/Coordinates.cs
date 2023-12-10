namespace InfiniteSokoban.Data;

public struct Coordinates(int x, int y)
{
    public int X => x;

    public int Y => y;

    public static Coordinates operator +(Coordinates a, Coordinates b) => new(a.X + b.X, a.Y + b.Y);
    public static Coordinates operator -(Coordinates a, Coordinates b) => new(a.X - b.X, a.Y - b.Y);
}
