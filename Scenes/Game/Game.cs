using InfiniteSokoban.Entities.Level;

namespace InfiniteSokoban.Scenes.Game;

public class Game : Node
{
    private Level _level = null!;
    
    public override void _Ready()
    {
        _level = GetNode<Level>("Level");
        
        // TODO: generate level based on settings.
        _level.Generate();
    }
}
