namespace InfiniteSokoban.Entities;

public class LevelEntity : Node2D
{
    private const int TileSize = 64;

    public Coordinates GridPosition
    {
        get => (X: (int)Position.x / TileSize, Y: (int)Position.y / TileSize);
        set => Position = new Vector2(value.X * TileSize, value.Y * TileSize);
    }
}
