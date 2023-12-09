using System.Text;

using InfiniteSokoban.Data;

namespace InfiniteSokoban.Globals.LevelGenerator;

public class GeneratedLevel : CoordinateArray<Cell>
{
    private GeneratedLevel(int width, int height) : base(width, height)
    {
    }

    public static GeneratedLevel Parse(string encodedLevel, int xRooms, int yRooms)
    {
        var lines = encodedLevel.Split('|');
        var height = yRooms * 3 + 2;
        var width = xRooms * 3 + 2;

        var level = new GeneratedLevel(width, height);

        foreach (var (row, y) in lines.Select((row, y) => (row, y)))
        {
            var num = string.Empty;
            var x = 0;
            foreach (var c in row)
            {
                if (char.IsDigit(c))
                {
                    num += c;
                }
                else
                {
                    var count = int.Parse(num);
                    num = string.Empty;
                    var cell = CellExtensions.FromChar(c);

                    for (var j = x; j < x + count; j++)
                    {
                        level[j, y] = cell;
                    }

                    x += count;
                }
            }
        }

        return level;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(Width * Height + Height);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++) sb.Append(this[y, x].ToChar());

            sb.Append('\n');
        }

        return sb.ToString();
    }
}
