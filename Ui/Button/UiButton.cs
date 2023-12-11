namespace InfiniteSokoban.Ui.Button;

public class UiButton : Godot.Button
{
    private AudioStreamPlayer _audioStreamPlayer = null!;
    
    public override void _Ready()
    {
        _audioStreamPlayer = GetNode<AudioStreamPlayer>("%Sfx");
    }
    
    private void OnPressed()
    {
        _audioStreamPlayer.Play();
    }
}
