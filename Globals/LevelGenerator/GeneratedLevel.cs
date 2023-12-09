using System.Text;

namespace InfiniteSokoban.Globals.LevelGenerator;

public class GeneratedLevel
{
    public Cell[,] Cells { get; private set; } = new Cell[0, 0];

    public int Width => Cells.GetLength(1);

    public int Height => Cells.GetLength(0);

    private GeneratedLevel()
    {
    }

    public static GeneratedLevel Parse(string encodedLevel)
    {
        var lines = encodedLevel.Split('|');
        var height = lines.Length;
        var width = int.Parse(lines[0][0].ToString());

        var level = new GeneratedLevel
        {
            Cells = new Cell[height, width],
        };

        foreach (var (row, y) in lines.Select((row, y) => (row, y)))
        {
            var x = 0;
            for (var i = 0; i < row.Length; i += 2)
            {
                var count = int.Parse(row[i].ToString());
                var cell = CellExtensions.FromChar(row[i + 1]);

                for (var j = x; j < x + count; j++)
                {
                    level.Cells[y, j] = cell;
                }

                x += count;
            }
        }

        return level;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(Width * Height + Height);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++) sb.Append(Cells[y, x].ToChar());

            sb.Append('\n');
        }

        return sb.ToString();
    }
}
