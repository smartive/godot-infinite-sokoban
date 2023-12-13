namespace InfiniteSokoban.LevelGenerator;

public enum Cell
{
    Empty,
    Wall,
    Player,
    PlayerOnGoal,
    Box,
    BoxOnGoal,
    Goal,
    Floor,
    SpecialFloor,
}

public static class CellExtensions
{
    public static bool IsGoal(this Cell cell) =>
        cell is Cell.Goal or Cell.PlayerOnGoal or Cell.BoxOnGoal;

    public static bool IsFloor(this Cell cell) =>
        cell is Cell.Floor or Cell.SpecialFloor;

    public static bool IsBox(this Cell cell) =>
        cell is Cell.Box or Cell.BoxOnGoal;

    public static bool IsWalkable(this Cell cell) =>
        cell is Cell.Floor or Cell.SpecialFloor or Cell.Goal;

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
            Cell.Floor or Cell.SpecialFloor => '-',
            Cell.Empty => 'X',
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null),
        };
}
