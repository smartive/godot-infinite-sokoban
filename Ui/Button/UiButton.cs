using InfiniteSokoban.Globals;
using InfiniteSokoban.Globals.UiAudio;

namespace InfiniteSokoban.Ui.Button;

public class UiButton : Godot.Button
{
    private UiAudio _audio = null!;

    public override void _EnterTree()
    {
        base._EnterTree();
        _audio = GetNode<UiAudio>(AutoLoadIdentifier.UiAudio);
    }

    private void OnPressed() => _audio.ButtonClick();
}
