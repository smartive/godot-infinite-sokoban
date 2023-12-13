using System.Text;

using InfiniteSokoban.Data;

namespace InfiniteSokoban.Globals.LevelGenerator;

public class GeneratedLevel(int width, int height) : CoordinateArray<Cell>(width, height, Cell.Wall)
{
    public override string ToString()
    {
        var sb = new StringBuilder(Width * Height + Height);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++) sb.Append(this[x, y].ToChar());

            sb.Append('\n');
        }

        return sb.ToString();
    }
}
