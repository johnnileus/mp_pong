using System.Linq;
using Godot;
using Godot.Collections;
using Array = System.Array;

[Tool]
public partial class Main : Node
{

    [Export] private PackedScene _startupScene;
    

    public override void _Ready()
    {
        // ... register stuff, etc.
        CallDeferred(nameof(GoToGame));
    }

    private void GoToGame()
    {
        GetTree().ChangeSceneToPacked(_startupScene);
    }
}