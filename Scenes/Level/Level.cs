using InfiniteSokoban.Globals.LevelGenerator;

namespace InfiniteSokoban.Scenes.Level;

public class Level : Node2D
{
    [Export(PropertyHint.Range, "1,4,1")]
    public int Width { get; set; } = 3;
    
    [Export(PropertyHint.Range, "1,4,1")]
    public int Height { get; set; } = 2;
    
    [Export(PropertyHint.Range, "1,3,1")]
    public int BoxCount { get; set; } = 2;

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _Ready()
    {
        GD.Print("OOO");
        base._Ready();

        var gen = new LevelGenerator(GetNode("LevelGenerator"));
        var l = gen.GenerateLevel(1,1,1);
        GD.Print(l);
    }
}
