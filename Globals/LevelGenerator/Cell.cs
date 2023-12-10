namespace InfiniteSokoban.Globals.LevelGenerator;

public enum Cell {
    Wall,
    Player,
    PlayerOnGoal,
    Box,
    BoxOnGoal,
    Goal,
    Floor,
}

public static class CellExtensions {
    public static bool BlocksPlayer(this Cell cell) =>
        cell switch {
            Cell.Wall => true,
            Cell.Box => true,
            Cell.BoxOnGoal => true,
            _ => false,
        };
    
    public static char ToChar(this Cell cell) =>
        cell switch {
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
        c switch {
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
