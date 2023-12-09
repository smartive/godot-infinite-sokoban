namespace InfiniteSokoban.Globals.LevelGenerator;

public class LevelGenerator : Node
{
    private Node _native = null!;

    public override void _Ready()
    {
        _native = GetNode("Native");
    }

    public GeneratedLevel GenerateLevel(int height, int width, int boxCount)
    {
        var encoded = _native.Call("generate_level", height, width, boxCount) as string;
        return GeneratedLevel.Parse(encoded ?? throw new("Level Null"), width, height);
    }
}
