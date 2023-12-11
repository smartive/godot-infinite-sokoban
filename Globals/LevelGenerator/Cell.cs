namespace InfiniteSokoban.Globals.LevelGenerator;

public enum Cell
{
    Wall,
    Player,
    PlayerOnGoal,
    Box,
    BoxOnGoal,
    Goal,
    Floor,
}

public static class CellExtensions
{
    public static bool IsGoal(this Cell cell) =>
        cell is Cell.Goal or Cell.PlayerOnGoal or Cell.BoxOnGoal;

    public static bool CouldBlockPlayer(this Cell cell) =>
        cell is Cell.Wall or Cell.Box or Cell.BoxOnGoal;

    public static char ToChar(this Cell cell) =>
        cell switch
        {
            Cell.Wall => '#',
            Cell.Player => '@',
            Cell.PlayerOnGoal => '+',
            Cell.Box => '$',
            Cell.BoxOnGoal => '*',
            Cell.Goal => '.',
            Cell.Floor => '-',
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null),
        };

    public static Cell FromChar(char c) =>
        c switch
        {
            '#' => Cell.Wall,
            '@' => Cell.Player,
            '+' => Cell.PlayerOnGoal,
            '$' => Cell.Box,
            '*' => Cell.BoxOnGoal,
            '.' => Cell.Goal,
            '-' => Cell.Floor,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null),
        };
}
