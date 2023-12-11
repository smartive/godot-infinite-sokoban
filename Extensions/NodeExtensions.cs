namespace InfiniteSokoban.Extensions;

public static class NodeExtensions
{
    public static void FreeAllChildren(this Node node)
    {
        foreach (Node child in node.GetChildren())
        {
            child.Free();
        }
    }
}
