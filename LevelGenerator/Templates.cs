using InfiniteSokoban.Data;

namespace InfiniteSokoban.LevelGenerator;

public static class Templates
{
    private static readonly Random Random = new();
    
    private static readonly CoordinateArray<Cell>[] RoomTemplates =
    {
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Floor, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Wall, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Floor, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Wall, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Floor, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Floor, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Wall, Cell.Empty, },
            { Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Wall, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Floor, Cell.Empty, Cell.Empty, },
        }),

        // This template is the exception for the
        // connectivity check.
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Floor, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Wall, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.SpecialFloor, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Floor, Cell.Wall, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Floor, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Floor, Cell.Floor, Cell.Empty, Cell.Empty, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Floor, Cell.Empty, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Floor, Cell.Wall, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Empty, Cell.Floor, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
        }),
        new(new[,]
        {
            { Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, Cell.Empty, },
            { Cell.Empty, Cell.Wall, Cell.Wall, Cell.Wall, Cell.Empty },
            { Cell.Floor, Cell.Floor, Cell.Wall, Cell.Floor, Cell.Floor, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Floor, Cell.Empty, },
            { Cell.Empty, Cell.Floor, Cell.Floor, Cell.Empty, Cell.Empty, },
        }),
    };

    public static CoordinateArray<Cell> GetRandomRoomTemplate()
    {
        var room = RoomTemplates[Random.Next(RoomTemplates.Length)];
        for (var x = 0; x < Random.Next(4); x++)
        {
            room.Rotate();
        }

        return room;
    }

    private static void Rotate(this CoordinateArray<Cell> template)
    {
        for (var x = 0; x < template.Width; x++)
        {
            for (var y = 0; y < template.Height; y++)
            {
                // TODO: check this.
                template.Swap((x, y), (y, template.Width - x - 1));
            }
        }
    }
}
