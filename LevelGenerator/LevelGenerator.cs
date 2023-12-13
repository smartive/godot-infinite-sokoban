using System.Text;

using InfiniteSokoban.Data;

namespace InfiniteSokoban.LevelGenerator;

using BacktrackState = (Coordinates[] Boxes, Coordinates PlayerPosition, CoordinateArray<Cell> Level, int Step);

public static class LevelGenerator
{
    private static readonly Random Random = new();

    public static GeneratedLevel GenerateLevel(int xRooms, int yRooms, int boxCount)
    {
        GeneratedLevel rooms;
        do
        {
            rooms = GenerateRooms(xRooms, yRooms);
        }
        while (!RoomsMeetRequirements(rooms, boxCount));

        CoordinateArray<Cell>? level;
        do
        {
            level = PlaceEntities(rooms, boxCount);
        }
        while (level is null);

        var framed = new GeneratedLevel(level.Width + 2, level.Height + 2);
        framed.ApplyWindow(level, (1, 1));

        return framed;
    }

    private static GeneratedLevel GenerateRooms(int xRooms, int yRooms)
    {
        var rooms = new GeneratedLevel(xRooms * 3, yRooms * 3);
        var filledWidth = 0;
        var filledHeight = 0;

        while (filledWidth < xRooms && filledHeight < yRooms)
        {
            var xBase = filledWidth * 3;
            var yBase = filledHeight * 3;
            var currentRoomEdges = new List<Cell>();

            if (filledWidth != 0)
            {
                currentRoomEdges.AddRange(rooms.GetWindow((xBase - 1, yBase), (xBase - 1, yBase + 2)));
            }

            if (filledWidth != xRooms - 1)
            {
                currentRoomEdges.AddRange(rooms.GetWindow((xBase + 3, yBase), (xBase + 3, yBase + 2)));
            }

            if (filledHeight != 0)
            {
                currentRoomEdges.AddRange(rooms.GetWindow((xBase, yBase - 1), (xBase + 2, yBase - 1)));
            }

            if (filledHeight != yRooms - 1)
            {
                currentRoomEdges.AddRange(rooms.GetWindow((xBase, yBase + 3), (xBase + 2, yBase + 3)));
            }

            CoordinateArray<Cell> newRoomTemplate;
            List<Cell> templateEdges;

            do
            {
                newRoomTemplate = Templates.GetRandomRoomTemplate();
                templateEdges = new List<Cell>();

                if (filledWidth != 0)
                {
                    // Add the left edge of the room to the list
                    templateEdges.AddRange(newRoomTemplate.GetWindow((0, 1), (0, 3)));
                }

                if (filledWidth != xRooms - 1)
                {
                    // Add the right edge of the room to the list
                    templateEdges.AddRange(newRoomTemplate.GetWindow((4, 1), (4, 3)));
                }

                if (filledHeight != 0)
                {
                    // Add the top edge of the room to the list
                    templateEdges.AddRange(newRoomTemplate.GetWindow((1, 0), (3, 0)));
                }

                if (filledHeight != yRooms - 1)
                {
                    // Add the bottom edge of the room to the list
                    templateEdges.AddRange(newRoomTemplate.GetWindow((1, 4), (3, 4)));
                }
            }
            while (!currentRoomEdges
                       .Zip(templateEdges,
                           (room, template) => room == Cell.Empty || template == Cell.Empty || room == template)
                       .All(x => x));

            var newRoom = newRoomTemplate.GetWindow((1, 1), (3, 3));
            rooms.ApplyWindow(newRoom, (filledWidth * 3, filledHeight * 3));

            if (filledWidth < xRooms - 1)
            {
                filledWidth += 1;
            }
            else
            {
                filledWidth = 0;
                filledHeight += 1;
            }
        }

        return rooms;
    }

    private static bool RoomsMeetRequirements(GeneratedLevel rooms, int boxCount)
    {
        return HasEnoughSpace()
               && HasNoSurroundedFloors()
               && HasEnoughGoalLocations()
               && HasNoLargeSpaces()
               && AllFloorsAreConnected();

        bool HasEnoughSpace()
        {
            var targetFloors = boxCount + 2;
            return rooms.Count(c => c.IsFloor()) >= targetFloors;
        }

        bool HasNoSurroundedFloors() =>
            rooms
                .IndexedIterator()
                .Where(c => c.Data.IsFloor())
                .All(c =>
                {
                    var (x, y, _) = c;
                    var surroundingWalls = 0;

                    if (x == 0 || rooms[x - 1, y] == Cell.Wall)
                    {
                        surroundingWalls += 1;
                    }

                    if (x == rooms.Width - 1 || rooms[x + 1, y] == Cell.Wall)
                    {
                        surroundingWalls += 1;
                    }

                    if (y == 0 || rooms[x, y - 1] == Cell.Wall)
                    {
                        surroundingWalls += 1;
                    }

                    if (y == rooms.Height - 1 || rooms[x, y + 1] == Cell.Wall)
                    {
                        surroundingWalls += 1;
                    }

                    return surroundingWalls < 3;
                });

        bool HasEnoughGoalLocations() => rooms.GetPossibleGoalLocations().Count() >= boxCount + 1;

        bool HasNoLargeSpaces()
        {
            foreach (var w in rooms.Windows(3, 4))
            {
                if (w.All(c => c.IsFloor()))
                {
                    return false;
                }
            }

            foreach (var w in rooms.Windows(4, 3))
            {
                if (w.All(c => c.IsFloor()))
                {
                    return false;
                }
            }

            return true;
        }

        bool AllFloorsAreConnected()
        {
            var startPoint = rooms.IndexedIterator().FirstOrDefault(c => c.Data == Cell.Floor);
            if (startPoint == default)
            {
                return false;
            }

            var visited = new CoordinateArray<bool>(rooms.Dimensions);
            var stack = new Stack<Coordinates>();
            stack.Push((startPoint.X, startPoint.Y));

            while (stack.Count > 0)
            {
                var (x, y) = stack.Pop();

                if (visited[x, y])
                {
                    continue;
                }

                visited[x, y] = true;

                if (x > 0 && rooms[x - 1, y] == Cell.Floor && !visited[x - 1, y])
                {
                    stack.Push((x - 1, y));
                }

                if (x < rooms.Width - 1 && rooms[x + 1, y] == Cell.Floor && !visited[x + 1, y])
                {
                    stack.Push((x + 1, y));
                }

                if (y > 0 && rooms[x, y - 1] == Cell.Floor && !visited[x, y - 1])
                {
                    stack.Push((x, y - 1));
                }

                if (y < rooms.Height - 1 && rooms[x, y + 1] == Cell.Floor && !visited[x, y + 1])
                {
                    stack.Push((x, y + 1));
                }
            }

            return rooms
                .Zip(visited, (r, v) => (Cell: r, visited: v))
                .Where(data => data.Cell.IsFloor())
                .All(data => data.visited);
        }
    }

    private static CoordinateArray<Cell>? PlaceEntities(GeneratedLevel rooms, int boxCount)
    {
        var level = rooms.Clone();
        foreach (var goal in GetRandomGoalLocations(level, boxCount))
        {
            level[goal] = Cell.BoxOnGoal;
        }

        CoordinateArray<Cell>? maxLevel = null;
        Coordinates? maxPos = null;
        var maxSteps = -1;

        foreach (var (calcedLevel, stepMap) in CreateBoxBacktrackMap(level).Values)
        {
            var max = stepMap.Max();
            if (max > maxSteps)
            {
                maxSteps = max;
                var (x, y, _) = stepMap.IndexedIterator().Last(d => d.Data == max);
                maxPos = (x, y);
                maxLevel = calcedLevel;
            }
        }

        if (maxLevel is null || maxPos is null)
        {
            return null;
        }

        maxLevel[maxPos.Value] = maxLevel[maxPos.Value] switch
        {
            Cell.Goal => Cell.PlayerOnGoal,
            _ => Cell.Player,
        };

        return maxLevel;
    }

    private static IEnumerable<Coordinates> GetRandomGoalLocations(CoordinateArray<Cell> rooms, int goalCount)
    {
        var goals = rooms.GetPossibleGoalLocations().ToArray();
        var n = goals.Length;
        while (n > 1)
        {
            var k = Random.Next(n--);
            (goals[n], goals[k]) = (goals[k], goals[n]);
        }

        return goals.Take(goalCount);
    }

    private static IEnumerable<Coordinates> GetPossibleGoalLocations(this CoordinateArray<Cell> level) =>
        level.IndexedIterator()
            .Where(c => c.Data.IsFloor())
            .Where(c =>
            {
                var (x, y, _) = c;

                // check if the cell has two collinear floors to any side

                if (x >= 2 && level[x - 1, y].IsFloor() && level[x - 2, y].IsFloor())
                {
                    return true;
                }

                if (x <= level.Width - 3 && level[x + 1, y].IsFloor() && level[x + 2, y].IsFloor())
                {
                    return true;
                }

                if (y >= 2 && level[x, y - 1].IsFloor() && level[x, y - 2].IsFloor())
                {
                    return true;
                }

                if (y <= level.Height - 3 && level[x, y + 1].IsFloor() && level[x, y + 2].IsFloor())
                {
                    return true;
                }

                return false;
            })
            .Select(c => (c.X, c.Y));

    private static Dictionary<string, (CoordinateArray<Cell> Level, CoordinateArray<int> Steps)> CreateBoxBacktrackMap(
        CoordinateArray<Cell> level)
    {
        var directions = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

        var possiblePlayerLocations = level.IndexedIterator().Where(c => c.Data.IsWalkable()).Select(c => (c.X, c.Y));
        var initialBoxes = level.IndexedIterator().Where(c => c.Data.IsBox()).Select(c => (c.X, c.Y)).ToList();

        var backtrack = new Dictionary<string, (CoordinateArray<Cell> Level, CoordinateArray<int> Steps)>();
        foreach (var (x, y) in possiblePlayerLocations)
        {
            var stack = new Stack<BacktrackState>();
            stack.Push((initialBoxes.ToArray(), (x, y), level.Clone(), 0));

            while (stack.Count > 0)
            {
                var state = stack.Pop();
                if (CheckForCachedMap(state))
                {
                    continue;
                }

                var (boxes, playerPos, currentLevel, step) = state;
                for (var i = 0; i < boxes.Length; i++)
                {
                    var (boxX, boxY) = boxes[i];
                    foreach (var (addX, addY) in directions)
                    {
                        var newBoxPos = (boxX + addX, boxY + addY);
                        if (!currentLevel.IsInBounds(newBoxPos) || !IsAccessible(currentLevel, playerPos, newBoxPos))
                        {
                            continue;
                        }

                        var newPlayerPos = (newBoxPos.Item1 + addX, newBoxPos.Item2 + addY);
                        if (!currentLevel.IsInBounds(newPlayerPos) ||
                            !IsAccessible(currentLevel, playerPos, newPlayerPos))
                        {
                            continue;
                        }

                        var newLevel = currentLevel.Clone();
                        newLevel[boxX, boxY] = currentLevel[boxX, boxY] switch
                        {
                            Cell.Box => Cell.Floor,
                            Cell.BoxOnGoal => Cell.Goal,
                            _ => throw new("Invalid Box Cell"),
                        };
                        newLevel[newBoxPos] = currentLevel[newBoxPos] switch
                        {
                            Cell.Floor => Cell.Box,
                            Cell.Goal => Cell.BoxOnGoal,
                            _ => throw new("Invalid Box Cell"),
                        };

                        var newBoxes = (Coordinates[])boxes.Clone();
                        newBoxes[i] = newBoxPos;

                        stack.Push((newBoxes, newPlayerPos, newLevel, step + 1));
                    }
                }
            }
        }

        return backtrack;

        bool CheckForCachedMap(BacktrackState state)
        {
            var (_, (playerX, playerY), currentLevel, step) = state;
            if (step == 0)
            {
                return false;
            }

            var identifier = currentLevel.Aggregate(new StringBuilder(), (sb, c) => sb.Append(c.ToChar())).ToString();

            if (backtrack.TryGetValue(identifier, out var cached))
            {
                if (cached.Steps[playerX, playerY] > 0)
                {
                    if (step < cached.Steps[playerX, playerY])
                    {
                        UpdateSteps(cached.Steps, playerX, playerY, step);
                    }

                    return true;
                }
            }
            else
            {
                backtrack.Add(identifier, (currentLevel, new CoordinateArray<int>(currentLevel.Dimensions)));
            }

            var (_, map) = backtrack[identifier];
            UpdateSteps(map, playerX, playerY, step);

            return false;

            void UpdateSteps(CoordinateArray<int> stepMap, int pX, int pY, int newStep)
            {
                var stack = new Stack<Coordinates>();
                stack.Push((pX, pY));

                while (stack.Count > 0)
                {
                    var (x, y) = stack.Pop();
                    if (stepMap[x, y] == newStep || !currentLevel[x, y].IsWalkable())
                    {
                        continue;
                    }

                    stepMap[x, y] = newStep;

                    if (x > 0)
                    {
                        stack.Push((x - 1, y));
                    }

                    if (x < currentLevel.Width - 1)
                    {
                        stack.Push((x + 1, y));
                    }

                    if (y > 0)
                    {
                        stack.Push((x, y - 1));
                    }

                    if (y < currentLevel.Height - 1)
                    {
                        stack.Push((x, y + 1));
                    }
                }
            }
        }

        bool IsAccessible(CoordinateArray<Cell> currentLevel, Coordinates origin, Coordinates target)
        {
            if (!currentLevel[origin].IsWalkable() || !currentLevel[target].IsWalkable() ||
                !currentLevel.IsInBounds(origin) || !currentLevel.IsInBounds(target))
            {
                return false;
            }

            var stack = new Stack<Coordinates>();
            stack.Push(origin);
            var visited = new CoordinateArray<bool>(currentLevel.Dimensions);

            while (stack.Count > 0)
            {
                var (x, y) = stack.Pop();
                if (visited[x, y])
                {
                    continue;
                }

                visited[x, y] = true;

                if (x == target.X && y == target.Y)
                {
                    return true;
                }

                if (x > 0 && currentLevel[x - 1, y].IsWalkable() && !visited[x - 1, y])
                {
                    stack.Push((x - 1, y));
                }

                if (x < currentLevel.Width - 1 && currentLevel[x + 1, y].IsWalkable() && !visited[x + 1, y])
                {
                    stack.Push((x + 1, y));
                }

                if (y > 0 && currentLevel[x, y - 1].IsWalkable() && !visited[x, y - 1])
                {
                    stack.Push((x, y - 1));
                }

                if (y < currentLevel.Height - 1 && currentLevel[x, y + 1].IsWalkable() && !visited[x, y + 1])
                {
                    stack.Push((x, y + 1));
                }
            }

            return false;
        }
    }
}
