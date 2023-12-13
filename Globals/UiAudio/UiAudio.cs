namespace InfiniteSokoban.Globals.UiAudio;

public class UiAudio : Node
{
    private AudioStreamPlayer _buttonClick = null!;

    public void ButtonClick() => _buttonClick.Play();
    
    public override void _Ready()
    {
        _buttonClick = GetNode<AudioStreamPlayer>("%ButtonClick");
    }
}
