namespace InfiniteSokoban.Globals.LevelGenerator;

public class LevelGenerator(Node native)
{
    public GeneratedLevel GenerateLevel(int width, int height, int boxCount)
    {
        var encoded = native.Call("generate_level", width, height, boxCount) as string;
        return GeneratedLevel.Parse(encoded ?? throw new("Level Null"));
    }
}
